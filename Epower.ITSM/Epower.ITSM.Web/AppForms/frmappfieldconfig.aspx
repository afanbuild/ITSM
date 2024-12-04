<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmAppFieldConfig.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmAppFieldConfig"
    Title="无标题页" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<%@ Register Src="~/Controls/ctrFlowCataDropListNew.ascx" TagName="ctrFlowCataDropListNew" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--******************************增加窗体代码********************************-->

    <script language="javascript" type="text/javascript">
		<!--

        function chkControl(obj) {
            var controlid = obj.id;

            objQ = document.getElementById(controlid + "Q");
            objF = document.getElementById(controlid + "F");
            //alert(controlid.substr(26));
            //objBtn = document.getElementById(controlid.substr(26)+"B");
            if (obj.checked == true) {

                objQ.disabled = false;
                objF.disabled = false;
                //objBtn.disabled = false;
            }
            else {
                objQ.disabled = true;
                objF.disabled = true;
                // objBtn.disabled = true;
            }

        }

        function ShowTable(imgCtrl) {
            var ImgPlusScr = "../Images/icon_expandall.gif";      	// pic Plus  +
            var ImgMinusScr = "../Images/icon_collapseall.gif";     // pic Minus - 
            var TableID = imgCtrl.id.replace("Img", "Table");
            var tableCtrl;
            tableCtrl = document.all.item(TableID);
            if (imgCtrl.src.indexOf("icon_expandall") != -1) {
                tableCtrl.style.display = "";
                imgCtrl.src = ImgMinusScr;
            }
            else {
                tableCtrl.style.display = "none";
                imgCtrl.src = ImgPlusScr;
            }
        }		
   //-->
    </script>

    <table style="width: 98%" class="listContent">
        <tr>
            <td class="listTitle" align="left" nowrap style="width: 60px; height: 16px">
                选择流程:
            </td>
            <td style="width: 326px;" nowrap colspan="6" align="left" class="list">
                <asp:DropDownList ID="dpdFlow" runat="server" Width="192px" AutoPostBack="True" OnSelectedIndexChanged="dpdFlow_SelectedIndexChanged">
                </asp:DropDownList>
                流程模型编号:<asp:Label ID="lblFlowModelID" runat="server" Text=""></asp:Label>
                
                
            </td>     
            
            <td class="listTitle" align="left" nowrap style="width: 70px;height: 16px">
            选择菜单组:
            </td>       
            
            <td style="width: 326px;" nowrap colspan="6" align="left" class="list">
                <uc1:ctrFlowCataDropListNew ID="ctrCateMenuGroup" runat="server" RootID="10262" />
            </td>
        </tr>     
    </table>
    <table width="98%" runat="server" class="listNewContent">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" alt="" id="Img1" onclick="ShowTable(this);" style="cursor: hand"
                                height="16" src="../Images/icon_collapseall.gif" width="16" align="absbottom" />
                            普通表单
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="listContent" width="98%" cellpadding="2" id="Table1" runat="server">
        <tr>
            <td align="left" class="listTitle" style="width: 100px; height: 16px">
                信息项
            </td>
            <td align="left" style="width: 150px; height: 16px" class="list">
            </td>
            <td align="left" style="width:450px;height: 16px" class="listTitle">
                      <table style="margin-left:10px;width:350px;">
                        <tbody>
                            <tr>
                                <td style="width:50px">显示</td>
                                <td style="width:50px">查询</td>
                                <td style="width:50px">跟踪</td>
                                <td style="width:50px">必填</td>
                                <td style="width:70px">显示<br />时间</td>
                                <td style="width:150px">分组</td>
                                <td style="width:100px">排序号</td>
                            </tr>
                        </tbody>
                    </table>   
            </td>
            <td align="left" class="listTitle" style="width: 100px; height: 16px">
                信息项
            </td>
            <td align="left" style="width: 150px; height: 16px" class="list">
            </td>
            <td align="left" style="height: 16px" class="listTitle" width="520">
                               <table style="margin-left:10px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:50px">显示</td>
                                <td style="width:50px">查询</td>
                                <td style="width:50px">跟踪</td>
                                <td style="width:50px">必填</td>
                                <td style="width:70px">显示<br />时间</td>
                                <td style="width:150px">分组</td>
                                <td style="width:100px">排序号</td>
                            </tr>
                        </tbody>
                    </table> 
            </td>
        </tr>
        <tr>
            <td style="width: 60px" class="listTitle" align="center">
                日期1
            </td>
            <td style="width: 100px" align="left" class="list">
                <asp:TextBox ID="txtDate1" runat="server" MaxLength="10" style="width: 100px">日期1</asp:TextBox>
            </td>
            <td align="left" class="list">
              <table border="0" style="margin-left:10px;width:350px;border:0px;">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate1" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate1Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate1F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate1M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate1S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate1Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate1Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>        
            </td>
            <td style="width: 60px" class="listTitle" align="center">
                日期2
            </td>
            <td style="width: 150px" align="left" class="list">
                <asp:TextBox ID="txtDate2" runat="server" MaxLength="10" style="width: 100px">日期2</asp:TextBox>
            </td>
            <td align="left" class="list" width="520">
             
              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate2" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate2Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate2F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate2M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate2S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate2Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate2Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td style="width: 60px" class="listTitle" align="center">
                日期3
            </td>
            <td style="width: 100px" align="left" class="list">
                <asp:TextBox ID="txtDate3" runat="server" MaxLength="10" style="width: 100px">日期3</asp:TextBox>
            </td>
            <td align="left" style="width:350px;" class="list">
            
              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate3" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate3Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate3F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate3M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate3S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate3Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate3Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
                    
            </td>
            <td style="width: 60px" class="listTitle" align="center">
                日期4
            </td>
            <td style="width: 150px" align="left" class="list">
                <asp:TextBox ID="txtDate4" runat="server" MaxLength="10" style="width: 100px">日期4</asp:TextBox>
            </td>
            <td align="left" class="list">
            
              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate4" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate4Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate4F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate4M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate4S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate4Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate4Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
                    
            </td>
        </tr>
        
    <tr>
            <td style="width: 60px" class="listTitle" align="center">
                日期5
            </td>
            <td style="width: 100px" align="left" class="list">
                <asp:TextBox ID="txtDate5" runat="server" MaxLength="10" style="width: 100px">日期5</asp:TextBox>
            </td>
            <td align="left" width="220" class="list">
              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate5" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate5Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate5F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate5M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate5S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate5Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate5Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td style="width: 60px" class="listTitle" align="center">
                日期6
            </td>
            <td style="width: 150px" align="left" class="list">
                <asp:TextBox ID="txtDate6" runat="server" MaxLength="10" style="width: 100px">日期6</asp:TextBox>
            </td>
            <td align="left" class="list">
              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate6" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate6Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate6F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate6M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate6S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate6Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate6Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
		
    <tr>
            <td style="width: 60px" class="listTitle" align="center">
                日期7
            </td>
            <td style="width: 100px" align="left" class="list">
                <asp:TextBox ID="txtDate7" runat="server" MaxLength="10" style="width: 100px">日期7</asp:TextBox>
            </td>
            <td align="left" width="220" class="list">
                      <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate7" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate7Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate7F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate7M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate7S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate7Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate7Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td style="width: 60px" class="listTitle" align="center">
                日期8
            </td>
            <td style="width: 150px" align="left" class="list">
                <asp:TextBox ID="txtDate8" runat="server" MaxLength="10" style="width: 100px">日期8</asp:TextBox>
            </td>
            <td align="left" class="list">
              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkDate8" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate8Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkDate8F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkDate8M" runat="server" /></td>
                                <td style="width:70px"><asp:CheckBox ID="chkDate8S" runat="server" /></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataDate8Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNDate8Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>		        
         
        <tr>
            <td class="listTitle" align="center">
                字符1
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtString1" runat="server" MaxLength="10"  style="width: 100px">字符1</asp:TextBox>
            </td>
            <td align="left" class="list">
                  <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString1" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString1Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString1F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString1M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString1Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString1Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
            </td>
            <td class="listTitle" align="center">
                字符2
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtString2" runat="server" MaxLength="10"  style="width: 100px">字符2</asp:TextBox>
            </td>
            <td align="left" class="list">
                  <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString2" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString2Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString2F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString2M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString2Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString2Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                字符3
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtString3" runat="server" MaxLength="10" style="width: 100px">字符3</asp:TextBox>
            </td>
            <td align="left" class="list">
                      <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString3" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString3Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString3F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString3M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString3Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString3Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle">
                字符4
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtString4" runat="server" MaxLength="10" style="width: 100px">字符4</asp:TextBox>
            </td>
            <td align="left" class="list">
                   <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString4" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString4Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString4F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString4M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString4Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString4Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle" style="height: 26px">
                字符5
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtString5" runat="server" MaxLength="10" style="width: 100px">字符5</asp:TextBox>
            </td>
            <td align="left" class="list">
                            <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString5" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString5Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString5F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString5M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString5Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString5聏rderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle" style="height: 26px">
                字符6
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtString6" runat="server" MaxLength="10" style="width: 100px">字符6</asp:TextBox>
            </td>
            <td align="left" class="list">
                         <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString6" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString6Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString6F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString6M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString6Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString6Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                字符7
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtString7" runat="server" MaxLength="10" style="width: 100px">字符7</asp:TextBox>
            </td>
            <td align="left" class="list">
                      <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString7" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString7Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString7F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString7M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString7Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString7Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle">
                字符8
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtString8" runat="server" MaxLength="10" style="width: 100px">字符8</asp:TextBox>
            </td>
            <td align="left" class="list">
                         <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkString8" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString8Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkString8F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkString8M" runat="server" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataString8Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNString8Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                数值1
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtNum1" runat="server" MaxLength="10" style="width: 100px">数值1</asp:TextBox>
            </td>
            <td align="left" class="list">
                              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkNum1" runat="server"></asp:CheckBox></td>
                                <td style="width:130px"></td>                                
                                <td style="width:130px"></td>
                                <td style="width:100px" align="center"><asp:CheckBox ID="chkNum1M" runat="server" />
								</td>
                                <td style="width:90px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataNum1Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNNum1Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle">
                数值2
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtNum2" runat="server" MaxLength="10" style="width: 100px">数值2</asp:TextBox>
            </td>
            <td align="left" class="list">
                  <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkNum2" runat="server"></asp:CheckBox></td>
                                <td style="width:130px"></td>                                
                                <td style="width:130px"></td>
                                <td style="width:100px" align="center"><asp:CheckBox ID="chkNum2M" runat="server" />
								</td>
                                <td style="width:80px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataNum2Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNNum2Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                数值3
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtNum3" runat="server" MaxLength="10" style="width: 100px">数值3</asp:TextBox>
            </td>
            <td align="left" class="list">
                              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkNum3" runat="server"></asp:CheckBox></td>
                                <td style="width:130px"></td>                                
                                <td style="width:130px"></td>
                                <td style="width:100px" align="center"><asp:CheckBox ID="chkNum3M" runat="server" />
								</td>
                                <td style="width:80px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataNum3Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNNum3Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle">
                数值4
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtNum4" runat="server" MaxLength="10" style="width: 100px">数值4</asp:TextBox>
            </td>
            <td align="left" class="list">
                              <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkNum4" runat="server"></asp:CheckBox></td>
                                <td style="width:130px"></td>                                
                                <td style="width:130px"></td>
                                <td style="width:100px" align="center"><asp:CheckBox ID="chkNum4M" runat="server" />
								</td>
                                <td style="width:80px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataNum4Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNNum4Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                数值5
            </td>
            <td align="left" style="width: 100px" class="list">
                <asp:TextBox ID="txtNum5" runat="server" MaxLength="10" style="width: 100px">数值5</asp:TextBox>
            </td>
            <td align="left" class="list">
                                 <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkNum5" runat="server"></asp:CheckBox></td>
                                <td style="width:130px"></td>                                
                                <td style="width:130px"></td>
                                <td style="width:100px" align="center"><asp:CheckBox ID="chkNum5M" runat="server" />
								</td>
                                <td style="width:80px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataNum5Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNNum5Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle">
                分类1
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCata1" runat="server" Width="100px" MaxLength="10" style="width: 100px">分类1</asp:TextBox><br />
                <uc1:ctrFlowCataDropList ID="CtrFlowCataDropList1" runat="server" RootID="1012" Width="100px" />
            </td>
            <td align="left" class="list">
                               <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkCata1" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata1Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkCata1F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata1M" runat="server" />
                <input id="chkCata1H" runat="server" name="chkCata1H" style="width: 56px" type="hidden" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataCata1Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNCata1Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>   
					
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                分类2
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCata2" runat="server" Width="100px" MaxLength="10" style="width: 100px">分类2</asp:TextBox> <br />
                <uc1:ctrFlowCataDropList ID="CtrFlowCataDropList2" runat="server" RootID="1012" Width="100px" />
            </td>
            <td align="left" class="list">
                       <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkCata2" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata2Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkCata2F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata2M" runat="server" />
								<input id="chkCata2H" runat="server" name="chkCata2H" style="width: 56px" type="hidden" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataCata2Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNCata2Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
					
            </td>
            <td align="center" class="listTitle">
                分类3
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCata3" runat="server" MaxLength="10" style="width: 100px">分类3</asp:TextBox><br />
                <uc1:ctrFlowCataDropList ID="CtrFlowCataDropList3" runat="server" RootID="1012" Width="100px" />
            </td>
            <td align="left" class="list">
                         <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkCata3" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata3Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkCata3F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata3M" runat="server" />
								<input id="chkCata3H" runat="server" name="chkCata3H" style="width: 56px" type="hidden" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataCata3Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNCata3Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
					
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                分类4
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCata4" runat="server" Width="100px" MaxLength="10" style="width: 100px">分类4</asp:TextBox><br />
                <uc1:ctrFlowCataDropList ID="CtrFlowCataDropList4" runat="server" RootID="1012" Width="100px" />
            </td>
            <td align="left" class="list">
                         <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkCata4" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata4Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkCata4F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata4M" runat="server" />
								<input id="chkCata4H" runat="server" name="chkCata4H" style="width: 56px" type="hidden" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataCata4Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNCata4Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
					
            </td>
            <td align="center" class="listTitle">
                分类5
            </td>
            <td align="left" class="list">
                <asp:TextBox ID="txtCata5" runat="server" Width="100px" MaxLength="10" style="width: 100px">分类5</asp:TextBox><br />
                <uc1:ctrFlowCataDropList ID="CtrFlowCataDropList5" runat="server" RootID="1012" Width="100px" />
            </td>
            <td align="left" class="list">
                           <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkCata5" onclick="chkControl(this);" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata5Q" runat="server" /></td>                                
                                <td style="width:100px"><asp:CheckBox ID="chkCata5F" runat="server" /></td>
                                <td style="width:100px"><asp:CheckBox ID="chkCata5M" runat="server" />
								<input id="chkCata5H" runat="server" name="chkCata5H" style="width: 56px" type="hidden" />
								</td>
                                <td style="width:70px">&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataCata5Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNCata5Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
					
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                判断1
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtBool1" runat="server" MaxLength="10" style="width: 100px">判断1</asp:TextBox>
            </td>
            <td align="left" class="list">
                                  <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkBool1" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"></td>                                
                                <td style="width:100px"></td>
                                <td style="width:100px"></td>
                                <td style="width:70px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataBool1Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNBool1Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
					
            </td>
            <td align="center" class="listTitle">
                判断2
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtBool2" runat="server" MaxLength="10" style="width: 100px">判断2</asp:TextBox>
            </td>
            <td align="left" class="list">
                                                  <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkBool2" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"></td>                                
                                <td style="width:100px"></td>
                                <td style="width:100px"></td>
                                <td style="width:70px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataBool2Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNBool2Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                判断3
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtBool3" runat="server" MaxLength="10" style="width: 100px">判断3</asp:TextBox>
            </td>
            <td align="left" class="list">
                                                 <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkBool3" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"></td>                                
                                <td style="width:100px"></td>
                                <td style="width:100px"></td>
                                <td style="width:70px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataBool3Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNBool3Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
            <td align="center" class="listTitle">
                判断4
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtBool4" runat="server" MaxLength="10" style="width: 100px">判断4</asp:TextBox>
            </td>
            <td align="left" class="list">
                                                  <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"><asp:CheckBox ID="chkBool4" runat="server"></asp:CheckBox></td>
                                <td style="width:100px"></td>                                
                                <td style="width:100px"></td>
                                <td style="width:100px"></td>
                                <td style="width:70px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataBool4Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNBool4Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                备注1
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="10" style="width: 100px">备注1</asp:TextBox>
            </td>
            <td align="left" colspan="4" class="list">
                                                 <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"> <asp:CheckBox ID="chkRemark" runat="server"></asp:CheckBox></td>
                                <td style="width:120px">&nbsp;&nbsp;&nbsp;</td>                                
                                <td style="width:120px">&nbsp;</td>
                                <td style="width:100px"><asp:CheckBox ID="chkRemarkM" runat="server"></asp:CheckBox>	</td>
                                <td style="width:70px"></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataRemarkGroup" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNRemarkOrderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
					
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                备注2
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtRemark2" runat="server" MaxLength="10" style="width: 100px">备注2</asp:TextBox>
            </td>
            <td align="left" colspan="4" class="list">
                                             <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"> <asp:CheckBox ID="chkRemark2" runat="server"></asp:CheckBox></td>
                                <td style="width:120px">&nbsp;&nbsp;&nbsp;</td>                                
                                <td style="width:120px">&nbsp;</td>
                                <td style="width:100px"><asp:CheckBox ID="chkRemark2M" runat="server"></asp:CheckBox>	</td>
                                <td style="width:70px"></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataRemark2Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNRemark2Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>
		
        <tr>
            <td align="center" class="listTitle">
                备注3
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtRemark3" runat="server" MaxLength="10" style="width: 100px">备注3</asp:TextBox>
            </td>
            <td align="left" colspan="4" class="list">
                                                            <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"> <asp:CheckBox ID="chkRemark3" runat="server"></asp:CheckBox></td>
                                <td style="width:120px">&nbsp;&nbsp;&nbsp;</td>                                
                                <td style="width:120px">&nbsp;</td>
                                <td style="width:100px"><asp:CheckBox ID="chkRemark3M" runat="server"></asp:CheckBox>	</td>
                                <td style="width:70px"></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataRemark3Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNRemark3Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>		
		
        <tr>
            <td align="center" class="listTitle">
                备注4
            </td>
            <td align="left" style="width: 150px" class="list">
                <asp:TextBox ID="txtRemark4" runat="server" MaxLength="10" style="width: 100px">备注4</asp:TextBox>
            </td>
            <td align="left" colspan="4" class="list">
                                                         <table border="0" style="margin-left:10px;border:0px;width:350px">
                        <tbody>
                            <tr>
                                <td style="width:100px"> <asp:CheckBox ID="chkRemark4" runat="server"></asp:CheckBox></td>
                                <td style="width:120px">&nbsp;&nbsp;&nbsp;</td>                                
                                <td style="width:120px">&nbsp;</td>
                                <td style="width:100px"><asp:CheckBox ID="chkRemark4M" runat="server"></asp:CheckBox>	</td>
                                <td style="width:70px"></td>
                                <td style="width:150px"><uc1:ctrFlowCataDropListNew  ID="ctrCataRemark4Group" runat="server" RootID="10262" ShowType="2" Width="100" /></td>
                                <td style="width:100px"><uc1:CtrFlowNumeric ID="ctrFNRemark4Orderby" runat="server" Width="50" /></td>
                            </tr>
                            
                        </tbody>
                    </table>  
            </td>
        </tr>				
                
    </table>
    <table id="Table12" width="98%" runat="server" class="listNewContent">
        <tr id="tr2" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" alt="" id="Img2" onclick="ShowTable(this);" style="cursor: hand"
                                height="16" src="../Images/icon_expandall.gif" width="16" align="absbottom" />
                            复杂表单
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="listContent" width="98%" cellpadding="2" id="Table2" style="display: none">
        <tr>
            <td align="left" class="listTitle" style="width: 60px">
                表单
            </td>
            <td align="left" class="list" style="width: 60px">
                <asp:TextBox ID="txtFbox" runat="server" MaxLength="10" style="width: 100px">复杂表单</asp:TextBox>
            </td>
            <td align="left" class="list" colspan="4">
                <ftb:FreeTextBox ID="ftxtDesc" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                    Height="300px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                </ftb:FreeTextBox>
            </td>
            <td align="left" class="list" style="width: 60px">
                &nbsp;<asp:CheckBox ID="chkDesc" runat="server"></asp:CheckBox>显示
                <br />
                <br />
                <br />
                <asp:RadioButtonList ID="rdblstDesc" runat="server" Visible="false">
                    <asp:ListItem Value="0">只读</asp:ListItem>
                    <asp:ListItem Value="1">编辑</asp:ListItem>
                    <asp:ListItem Value="2" Selected="true">流程控制</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <table width="98%" runat="server" class="listNewContent">
        <tr id="tr3" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" alt="" id="Img3" onclick="ShowTable(this);" style="cursor: hand"
                                height="16" src="../Images/icon_expandall.gif" width="16" align="absbottom" />
                            打印设置
                        </td>
            </tr> </table> </td>
        </tr>
    </table>
    <table class="listContent" width="98%" id="Table3" style="display: none">
        <tr>
            <td align="left" class="listTitle" style="width: 60px">
                打印头部
            </td>
            <td align="left" class="list" colspan="4">
                <ftb:FreeTextBox ID="ftxtTitle" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                    Height="300px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                </ftb:FreeTextBox>
            </td>
            <td align="left" class="list" style="width: 60px">
                &nbsp;<asp:CheckBox ID="chkTitle" runat="server"></asp:CheckBox>启用
            </td>
        </tr>
        <tr>
            <td align="center" class="listTitle">
                打印底部
            </td>
            <td align="left" class="list" colspan="4">
                <ftb:FreeTextBox ID="ftxtBottom" runat="server" ButtonPath="../Forms/images/epower/officexp/"
                    Height="300px" ImageGalleryPath="Attfiles\\Photos" Width="100%">
                </ftb:FreeTextBox>
            </td>
            <td align="left" class="list">
                &nbsp;<asp:CheckBox ID="chkBottom" runat="server"></asp:CheckBox>启用
            </td>
        </tr>
    </table>
    <asp:Label ID="labMsg" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
