<%@ Page Title="������Ϣ�༭" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="inputnew.aspx.cs" Inherits="Epower.ITSM.Web.Forms.inputnew" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../Js/App_Common.js"> </script>

    <script language="javascript" src="../Controls/Calendar/Popup.js"></script>
    <script language="javascript" type="text/javascript">
        function MustValid() { 
            var sMsg = "";
            if (document.all.<%=TxtNewsTitle.ClientID%>.value.trim()=="")        
	        {
	            document.all.<%=TxtNewsTitle.ClientID%>.focus();
		        sMsg = "��Ϣ��Ŀ";
	        }
	        if (document.all.<%=TxtNewsWriter.ClientID%>.value.trim()=="")        
	        {
	            document.all.<%=TxtNewsWriter.ClientID%>.focus();
		        sMsg = sMsg + ",������ ";
	        }
	        var lngSelected = document.all.<%=DpDNewType.ClientID%>.selectedIndex;
	        if(lngSelected==0)
	        {
	             document.all.<%=DpDNewType.ClientID%>.focus();
		         sMsg = sMsg + ",��Ϣ��� ";
	        }
	        	        
	        if(document.all.<%=ctrTxtNewsPubdate.ClientID%>_txtDate.value=="")
	        {
	            document.all.<%=ctrTxtNewsPubdate.ClientID%>_txtDate.focus();
	            sMsg = sMsg + ",����ʱ�� ";
	        }
	        
	        if(document.all.<%=strtxtOutDate.ClientID%>_txtDate.value=="")
	        {
	            document.all.<%=strtxtOutDate.ClientID%>_txtDate.focus();
	            sMsg = sMsg + ",��ֹʱ�� ";
	        }
	        
			//var UserID = document.all.lstSelected.options(lngSelected).value;
	        if(sMsg!="")
	        {
	            if(sMsg.substr(0,1)==",")
	               sMsg = sMsg.substr(1);
	            
	            alert(sMsg + " ����Ϊ�գ�");
	            event.returnValue = false;
	        }
        }
        String.prototype.trim = function()  //ȥ�ո�
			{
				// ��������ʽ��ǰ��ո�
				// �ÿ��ַ��������
				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
    </script>

    <div style="height: 10">
    </div>
    <table id="Table1" width="98%" align="center" class="listContent" cellpadding="2" cellspacing="0">
        <tr>
            <td align="center" colspan="2" class="listTitle">
                <input id="Hidden1" type="hidden" value="Attfiles\news" name="CurrentImagesFolder"
                    runat="server">
                <asp:Button ID="Button1" runat="server" Text="��  ��" OnClick="BtnSub_ok_Click" CssClass="btnClass" OnClientClick="MustValid();">
                </asp:Button>                
                   <asp:Button ID="btnCancel2" Text="ȡ  ��" runat="server" CssClass="btnClass" 
                    OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td style="width: 12%" class="listTitleRight">
              <asp:Literal runat="server" ID="litTitle" Text="��Ϣ��Ŀ"></asp:Literal>
            </td>
            <td class="list">
                <asp:TextBox ID="TxtNewsTitle" runat="server" MaxLength="300"></asp:TextBox>
                <asp:CheckBox ID="chkBulletin" runat="server" Text="���¹���" Visible="false" Checked="true">
                </asp:CheckBox><font color="red">*</font>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
              <asp:Literal runat="server" ID="litTypeId" Text="��Ϣ���"></asp:Literal>   
            </td>
            <td class="list">                
                <asp:DropDownList ID="DpDNewType" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:Label ID="labMsg" runat="server" ForeColor="Red"></asp:Label><font color="red">*</font>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
               <asp:Literal runat="server" ID="litDispFlag" Text="�Ƿ���ʾ"></asp:Literal>   
            </td>
            <td class="list">
                <asp:DropDownList ID="DrpDispFlag" runat="server" Width="152px">
                    <asp:ListItem Value="1">��</asp:ListItem>
                    <asp:ListItem Value="0">��</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                 <asp:Literal runat="server" ID="litIsAlert" Text="�Ƿ񵯳�"></asp:Literal>   
            </td>
            <td class="list">
                <asp:DropDownList ID="DrIsAlert" runat="server" Width="152px">                    
                    <asp:ListItem Value="0">��</asp:ListItem>
                    <asp:ListItem Value="1">��</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <asp:Literal runat="server" ID="litWriter" Text="������"></asp:Literal>      
            </td>
            <td class="list">
                <asp:TextBox ID="TxtNewsWriter" runat="server" MaxLength="100"></asp:TextBox><font
                    color="red">*</font>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
               <asp:Literal runat="server" ID="litIsInner" Text="��ʾ��Χ"></asp:Literal>       
            </td>
            <td class="list">
                <asp:DropDownList ID="DrpIsInner" runat="server" Width="152px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                 <asp:Literal runat="server" ID="litPubDate" Text="����ʱ��"></asp:Literal>     
            </td>
            <td class="list" >
                <uc2:ctrdateandtime ID="ctrTxtNewsPubdate" runat="server" MustInput="true" TextToolTip="����ʱ��" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
               <asp:Literal runat="server" ID="litOutDate" Text="��ֹʱ��"></asp:Literal>        
            </td>
            <td class="list">
                <uc2:ctrdateandtime ID="strtxtOutDate" runat="server" MustInput="true" TextToolTip="��ֹʱ��" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
               <asp:Literal runat="server" ID="litContent" Text="��������"></asp:Literal>     
            </td>
            <td class="list">
                <ftb:FreeTextBox ID="FreeTextBox1" runat="server" Width="100%" ButtonPath="./images/epower/officexp/"
                    ImageGalleryPath="Attfiles\\Photos" Height="600px">
                </ftb:FreeTextBox>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                <font face="����">����</font>
            </td>
            <td class="list">
                <uc1:ctrattachment ID="Ctrattachment1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" class="listTitle">
                <input id="CurrentImagesFolder" type="hidden" value="Attfiles\news" name="CurrentImagesFolder"
                    runat="server">
                <asp:Button ID="BtnSub_ok" runat="server" Text="��  ��" OnClick="BtnSub_ok_Click" CssClass="btnClass"  OnClientClick="MustValid();">
                </asp:Button>                                
                <asp:Button ID="btnCancel" Text="ȡ  ��" runat="server" CssClass="btnClass" 
                    OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    <div style="visibility: hidden">
        <asp:CheckBox ID="ChkFocusNews" runat="server" Text="��������"></asp:CheckBox>
    </div>
</asp:Content>
