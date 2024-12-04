<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="True" CodeBehind="frmzhCstMaterial.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frmzhCstMaterial" Title="无标题页" %>

<%@ Register Src="../Controls/ctrdateandtime.ascx" TagName="ctrdateandtime" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register TagPrefix="igchart" Namespace="Infragistics.WebUI.UltraWebChart" Assembly="Infragistics.WebUI.UltraWebChart.v5.1, Version=5.1.20051.37, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igchartprop" Namespace="Infragistics.UltraChart.Resources.Appearance" Assembly="Infragistics.UltraChart.Resources.v5.1, Version=5.1.20051.37, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc6" %>
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
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
					<TR>
						<TD align="left" style="height: 23px">时间范围:
                            <asp:TextBox ID="txtBeginDate" runat="server" MaxLength="80" Width="128px"></asp:TextBox><asp:Image
                                ID="imgBeginDate" runat="server" ImageUrl="../Controls/Calendar/calendar.gif"
                                Style="cursor: hand" />
                            --
                            <asp:TextBox ID="txtEndDate" runat="server" MaxLength="80" Width="128px"></asp:TextBox><asp:Image
                                ID="imgEndDate" runat="server" ImageUrl="../Controls/Calendar/calendar.gif" Style="cursor: hand" />&nbsp;&nbsp;<asp:Label ID="labManageOffice" runat="server" Text="责任部门:" Visible="false"></asp:Label>&nbsp;
								<asp:dropdownlist id="dpdManageOffice" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdManageOffice_SelectedIndexChanged"  Visible="false"></asp:dropdownlist>&nbsp;&nbsp; 事件类别:
                            <uc6:ctrFlowCataDropList id="ctrFCDServiceType" runat="server" RootID="1001">
                            </uc6:ctrFlowCataDropList>
                            &nbsp; &nbsp; 事件状态
                            <uc6:ctrFlowCataDropList ID="CtrFCDDealStatus" runat="server" RootID="1017" />
                         &nbsp;&nbsp;
								<asp:Literal  ID="LitMastShortName" runat="server" Text="服务单位"></asp:Literal>
								<asp:dropdownlist id="dpdMastCustomer" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="dpdMastCustomer_SelectedIndexChanged" ></asp:dropdownlist>
								</TD>
					</TR>
					<TR>
						<TD vAlign="top" align="left" width="100%" class="listTitle">
                              <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif" width="16"/>展示数据
						</TD>
					</TR>
					<TR id="tr1" runat="server">
						<TD align="left">
							<table id="TABLE2" runat="server" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD align="left" class="listContent" colspan="2">
										<asp:DataGrid id="dgMaterialStat" runat="server" cellpadding="1" cellspacing="2" BorderWidth="0px" Width="100%" AutoGenerateColumns="True" AllowCustomPaging="True" PageSize="100">
											    <AlternatingItemStyle BackColor="Azure"></AlternatingItemStyle>
                                                <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                                                <HeaderStyle CssClass="listTitle"></HeaderStyle>
										</asp:DataGrid>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>

</asp:Content>
