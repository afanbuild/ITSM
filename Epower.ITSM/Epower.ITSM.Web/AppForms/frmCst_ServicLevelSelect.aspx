<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmCst_ServicLevelSelect.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmServicLevelSelect"
    Title="服务级别" %>

<%@ Register TagPrefix="uc1" TagName="ControlPage" Src="../Controls/ControlPage.ascx" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
    .listContent {
 background-color:#CEE3F2;
 
}
</style>
<link href="../App_Themes/NewOldMainPage/css.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        
         
        function OpenWindow(url) {
            var openobj = window;
            if (typeof (window.dialogArguments) == "object") {
                openobj = window.dialogArguments;
            }
            openobj.open(url, "_blank", "scrollbars=yes,status=yes ,resizable=yes,width=700,height=480");

        }
        //对应的父页面是CST_Issue_Service.aspx ======zxl==
        function CST_Issue_Service_Select(value1, value2, value3, value4)
        {
       // debugger;
            var value = new Array();
            value[0] = value1;
            value[1] = value2;
            value[2] = value3;
            value[3] = value4;        
            if(value != null)
				{
					if(value.length>0)
					{
			                    
			            window.opener.document.getElementById("<%=Opener_ClientId %>trServiceLevelDetail").style.display = "";   //整体显示  
			                  
			           window.opener.document.getElementById("<%=Opener_ClientId %>txtServiceLevel").value = value[1];   //级别名称        
                       window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevel").value = value[1];   //级别名称
			          window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevelID").value = value[0];  //级别ID
			            window.opener.document.getElementById("<%=Opener_ClientId %>divSLDefinition").innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + value[2].replace(/@/g,";") + "</td></tr></table>";   //级别定义
			            
			            window.opener.document.getElementById("<%=Opener_ClientId %>divSLTimeLimt").innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle'  width='15%'>时限要求</td><td class='list'>" + value[3]+ "</td></tr></table>";  //级别时限
			            
			            window.opener.document.getElementById("<%=Opener_ClientId %>lnkServiceLevel").innerText = "详情";
			            
			            window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevelChange").value = "true";  //更改过服务级别
			            
					}
				}
				window.close();
            
        }
        
        function SErverLeveSelect(value1, value2, value3, value4)
        {
              var arr = new Array();
            arr[0] = value1;
            arr[1] = value2;
            arr[2] = value3;
            arr[3] = value4;
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_trShowServiceLevel").style.display="";
         
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtServiceLevel").value = arr[1];   //级别名称
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidServiceLevel").value = arr[1];   //级别名称
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidServiceLevelID").value = arr[0];  //级别ID

            window.close();
        
        }
        function ServerLevelOndblclick(value1, value2, value3, value4) {
         
            var arr = new Array();
            arr[0] = value1;
            arr[1] = value2;
            arr[2] = value3;
            arr[3] = value4;
           // window.opener.document.getElementById("ctl00_ContentPlaceHolder1_trShowServiceLevel").style.display="";
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_trServiceLevelDetail").style.display = "";   //整体显示
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtServiceLevel").value = value[1];   //级别名称
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidServiceLevel").value = value[1];   //级别名称
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidServiceLevelID").value = value[0];  //级别ID
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_divSLDefinition").innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + value[2].replace(/@/g, ";") + "</td></tr></table>";   //级别定义

            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_divSLTimeLimt").innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle'  width='15%'>时限要求</td><td class='list'>" + value[3] + "</td></tr></table>";  //级别时限

            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_lnkServiceLevel").innerText = "详情";

            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_hidServiceLevelChange").value = "true";  //更改过服务级别

            window.close();           
        }
        function CST_Issue_Base(value1, value2, value3, value4)
        {
        
			             window.opener.document.getElementById("<%=Opener_ClientId %>trServiceLevelDetail").style.display = "";   //整体显示        
			             window.opener.document.getElementById("<%=Opener_ClientId %>txtServiceLevel").value = value2;   //级别名称        
                         window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevel").value = value2;   //级别名称
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevelID").value = value1;  //级别ID
			             window.opener.document.getElementById("<%=Opener_ClientId %>divSLDefinition").innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle' width='15%'>级别定义</td><td class='list'>" + value3.replace(/@/g,";") + "</td></tr></table>";   //级别定义
			            
			             window.opener.document.getElementById("<%=Opener_ClientId %>divSLTimeLimt").innerHTML = "<table class='listContent' width='100%'> <tr><td class='listTitle'  width='15%'>时限要求</td><td class='list'>" + value4+ "</td></tr></table>";  //级别时限
			            
			             window.opener.document.getElementById("<%=Opener_ClientId %>lnkServiceLevel").innerText = "详情";
			            
			             window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevelChange").value = "true";  //更改过服务级别
			            
				  window.close();
            
            }
         //事件请求模板调用  yxq
         function frm_Issue_Template(value1, value2, value3, value4)
         {
        
			 window.opener.document.getElementById("<%=Opener_ClientId %>txtServiceLevel").value = value2;   //级别名称        
			 window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevel").value = value2;   //级别名称        
			 window.opener.document.getElementById("<%=Opener_ClientId %>hidServiceLevelID").value = value1;  //级别ID
			
		     window.close();
            
         }
         function showService(obj)
         {
         var id=obj.id.replace("lnkLook","lblID");
         var value=document.getElementById(id).innerText;
            var url="frmCst_ServiceLevelEdit.aspx?IsSelect=1&id="+value;
			window.open(url,"","resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=600,height=400,left=400,top=300");
					   
         }
         
        
    </script>

 
    <table cellpadding='2' cellspacing='0' width='98%'>
        <tr>
            <td align="right">
                <asp:Button ID="cmdCondQuery" CssClass="btnClass" runat="server" Text="规则级别" OnClick="cmdCondQuery_Click" />
                <asp:Button ID="cmdAllQuery" CssClass="btnClass" runat="server" OnClick="cmdAllQuery_Click"
                    Text="全部级别" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <table style="width: 100%" class="listContent"  cellpadding='2'  cellspacing='1'> <%--class="listContent"--%>
                    <tr>
                        <td class="listTitleRight" style="width: 12%;">
                            服务单位
                        </td>
                        <td class="list" style="width: 35%;">
                            <asp:Label ID="labMastCust" runat="server"></asp:Label>
                        </td>
                        <td class="listTitleRight" style="width: 12%;">
                            客户名称
                        </td>
                        <td class="list">
                            <asp:Label ID="labCust" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            资产名称
                        </td>
                        <td class="list">
                            <asp:Label ID="labEqu" runat="server"></asp:Label>
                        </td>
                        <td class="listTitleRight">
                            事件类别
                        </td>
                        <td class="list">
                            <asp:Label ID="labType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="listTitleRight">
                            事件性质
                        </td>
                        <td class="list">
                            <asp:Label ID="labKind" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="listTitleRight">
                            影响度
                        </td>
                        <td class="list">
                            <asp:Label ID="labEff" runat="server"></asp:Label>
                        </td>
                        <td class="listTitleRight">
                            紧急度
                        </td>
                        <td class="list">
                            <asp:Label ID="labIns" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 597px; font-weight: bold" class="listTitleNoAlign" align="center">
                <asp:Label ID="labMsg" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="98%" border="0">
        <tr>
            <td>
                <asp:DataGrid ID="dgCst_ServiceLevel" runat="server" Width="100%" CellPadding="1"
                    CellSpacing="2" BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgCst_ServiceLevel_ItemCommand"
                    OnItemCreated="dgCst_ServiceLevel_ItemCreated" OnItemDataBound="dgCst_ServiceLevel_ItemDataBound">
                    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <%--<asp:BoundColumn DataField='ID' HeaderText='ID' Visible="False"></asp:BoundColumn>--%>
                        <asp:TemplateColumn HeaderText="级别名称">
                            <ItemTemplate>
                            <asp:Label ID="lblID" CssClass="none" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID")%>'></asp:Label>
                            <asp:Label ID="lblLevelName"  runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LevelName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                       <asp:BoundColumn DataField='LevelName' HeaderText='级别名称' Visible="false">
                            <HeaderStyle Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField='Definition' HeaderText='级别定义'>
                            <HeaderStyle Width="30%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText='相关指标'>
                            <HeaderStyle Width="30%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="选择">
                            <ItemTemplate>
                                <asp:Button ID="lnkSelect" SkinID="btnClass1" runat="server" Text="选择" CommandName="Select" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="详情">
                            <ItemTemplate>
                                <asp:Button ID="lnkLook" SkinID="btnClass1" runat="server" Text="详情" OnClientClick="showService(this)"  CommandName="Look" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField='BaseLevel' HeaderText='服务包括' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='NotInclude' HeaderText='服务不包括' Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField='Availability' HeaderText='服务有效性' Visible="False"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:ControlPage ID="ControlPage1" runat="server"></uc1:ControlPage>
            </td>
        </tr>
    </table>
</asp:Content>
