<%@ Page Title="进出操作间和机房" Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frmOA_NEWS.aspx.cs" Inherits="Epower.ITSM.Web.Forms.frmOA_NEWS" ValidateRequest="false" %>

<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrattachment.ascx" TagName="ctrattachment" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="CtrDateAndTimeV2" TagPrefix="uc2" %>
<%@ Register src="../Controls/UEditor.ascx" tagname="UEditor" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript">
        //
        function TransferValue() {
           if (typeof(document.all.<%=txtTitle.ClientID%>)!="undefined" )
           {
              if(document.all.<%=txtTitle.ClientID%>.value.trim()!="")
              {
	                parent.header.flowInfo.Subject.value = "公告发布--"+document.all.<%=txtTitle.ClientID%>.value.trim();
	          }
	       }
          
        }

        //用户验证        
        function DoUserValidate(lngActionID, strActionName) {
            TransferValue();
            return CheckCustAndType();
        }

        //获取随机数

        function GetRandom() {
            return Math.floor(Math.random() * 1000 + 1);
        }

        //去空格    
        String.prototype.trim = function() {
            // 用正则表达式将前后空格

            // 用空字符串替代。

            return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;", "");
        }

        //检查是否为空

        function CheckCustAndType() {
              var sMsg = "";
            if(typeof(document.all.<%=txtTitle.ClientID%>)!="undefined")
            {
                if (document.all.<%=txtTitle.ClientID%>.value.trim()=="")        
	            {
	                document.all.<%=txtTitle.ClientID%>.focus();
		            sMsg = "信息题目,";
	            }
	        }
	        if(typeof(document.all.<%=txtWriter.ClientID%>)!="undefined")
            {
	            if (document.all.<%=txtWriter.ClientID%>.value.trim()=="")        
	            {
	                document.all.<%=txtWriter.ClientID%>.focus();
		            sMsg = sMsg + "发布人,";
	            }
	        }
	        if(typeof(document.all.<%=ddlType.ClientID%>)!="undefined")
            {
	            var lngSelected = document.all.<%=ddlType.ClientID%>.selectedIndex;
	            if(lngSelected==0)
	            {
	                 document.all.<%=ddlType.ClientID%>.focus();
		             sMsg = sMsg + "信息类别 ";
	            }
	        }
	        if(sMsg!="")
	        {
	            alert(sMsg + " 不能为空！");
	            return false;
	        }	        
            return true;
        }

    </script>

    <table id="Table1" width="100%" align="center" class="listContent" cellpadding="1" border="0"
        cellspacing="1">
        <tr>
            <td style="width: 12%" class="listTitleRight">
            <asp:Literal runat="server" ID="OA_Title" Text="信息题目"></asp:Literal>
            </td>
            <td class="list" colspan="3">
                <asp:TextBox ID="txtTitle" runat="server" Width="85%" MaxLength="300"></asp:TextBox>
                <asp:Label ID="lblTitle" runat="server" Text="" Visible="false"></asp:Label>
                <asp:CheckBox ID="chkBulletin" runat="server" Text="最新公告" Visible="false" Checked="true">
                </asp:CheckBox>
                <asp:Label ID="lblTitleWarning" runat="server" Font-Bold="False" Font-Size="Small"
                    ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
             <asp:Literal runat="server" ID="OA_TypeName" Text="信息类别"></asp:Literal>   
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlType" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:Label ID="lblType" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lblTypeWarning" runat="server" Font-Bold="False" Font-Size="Small"
                    ForeColor="Red">*</asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal runat="server" ID="OA_DispFlag" Text="是否显示"></asp:Literal>   
                
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlDispFlag" runat="server" Width="152px">
                    <asp:ListItem Value="0">否</asp:ListItem>
                    <asp:ListItem Value="1">是</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblDispFlag" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
             <asp:Literal runat="server" ID="OA_IsAlert" Text="是否弹出"></asp:Literal>   
                
            </td>
            <td class="list" style="width: 35%">
                <asp:DropDownList ID="ddlIsAlert" runat="server" Width="152px">
                    <asp:ListItem Value="0">否</asp:ListItem>
                    <asp:ListItem Value="1">是</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblIsAlert" runat="server" Text="" Visible="false"></asp:Label>
            </td>
            <td class="listTitleRight" style="width: 12%">
              <asp:Literal runat="server" ID="OA_Writer" Text="发布人"></asp:Literal>      

            </td>
            <td class="list">
                <asp:TextBox ID="txtWriter" runat="server" MaxLength="100"></asp:TextBox>
                <asp:Label ID="lblWriter" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lblWriterWarning" runat="server" Font-Bold="False" Font-Size="Small"
                    ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
               <asp:Literal runat="server" ID="OA_PubDate" Text="发布时间"></asp:Literal>     
            </td>
            <td class="list" style="width: 35%">
                <uc2:CtrDateAndTimeV2 ID="ctrPubDate" runat="server" MustInput="true" TextToolTip="发布时间" />
            </td>
            <td class="listTitleRight" style="width: 12%">
              <asp:Literal runat="server" ID="OA_OutDate" Text="截止时间"></asp:Literal>        
            </td>
            <td class="list">
                <uc2:CtrDateAndTimeV2 ID="ctrOutDate" runat="server" MustInput="true" TextToolTip="截止时间" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
             <asp:Literal runat="server" ID="OA_ISINNER" Text="显示范围"></asp:Literal>       
               
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="ddlIsInner" runat="server" Width="152px">
                </asp:DropDownList>
                <asp:Label ID="lblIsInner" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight" style="width: 12%">
                <asp:Literal runat="server" ID="OA_Content" Text="具体内容"></asp:Literal>       
            </td>
            <td class="list ueditor_tbl" colspan="3">
                <uc5:UEditor ID="UEditor1" runat="server" UEditorFrameWidth="800" />
                <asp:Label ID="lblContent" runat="server" Text="" Visible="false"></asp:Label>
                
            </td>
        </tr>
    </table>
    <div style="visibility: hidden">
        <asp:CheckBox ID="ChkFocusNews" runat="server" Text="焦点新闻"></asp:CheckBox>
    </div>
</asp:Content>
