<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
 CodeBehind="frm_AssetsAvailability.aspx.cs" Inherits="Epower.ITSM.Web.AnalsysForms.frm_AssetsAvailability" 
 Title="资产可用率报告" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>


<%@ Register src="../Controls/ctrDateSelectTime.ascx" tagname="ctrDateSelectTime" tagprefix="uc1" %>
<%@ Register src="../Controls/ctrDateSelectTimeV2.ascx" tagname="ctrDateSelectTime" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" language="javascript">
            var hidListID;
            var listname;
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
           
			//设备
			function SelectEqu(obj) 
			{
			    var EquipmentCatalogID = "0";
			    var EquName = document.all.<%=txtEqu.ClientID%>.value.trim();	
			    var CustName ="";
			    var MastCust = "";
			    var subjectid="010007";//类别为应用系统的 
				
				var	value=window.showModalDialog("../EquipmentManager/frmEqu_DeskMain.aspx?IsSelect='1'&randomid="+GetRandom()+"&FlowID="+ '' + "&EquName=" + escape(EquName) + "&Cust=" + escape(CustName) +"&subjectid="+subjectid+"&MastCust=" + escape(MastCust)+"&EquipmentCatalogID="+EquipmentCatalogID,window,"dialogWidth=800px; dialogHeight=600px;status=no; help=no;scroll=auto;resizable=no") ;
				if(value != null)
				{
					var json = value;
				    var record=json.record;				    
					for(var i=0; i < record.length; i++)
					{				
			            listname= record[i].name;   //资产目录名称
			           
			            document.getElementById("ctl00_ContentPlaceHolder1_txtEqu").innerText=listname;
			            document.getElementById("ctl00_ContentPlaceHolder1_HidAvaEqu").value= record[i].id;
					}
				}
				else
				{
				    listname="";
				}		
			}
    </script>
    <input id="HidAvaEqu" type="hidden" runat="server" />
    <table id="Table1" width="98%" class="listContent" cellpadding="2">
        <tr>            
             <td class="listTitleRight" width="12%"  id="tdtime1"  runat="server">
                时间范围:
            </td>
            <td class="list" id="tdtime2"  runat="server">
              
               <uc2:ctrDateSelectTime ID="CtrDateSelectTime1" runat="server"  />          
            </td>
            <td align="center" class="listTitleRight" width="12%" >
               <asp:Literal ID="LitEquipmentName" runat="server" Text="资产名称"></asp:Literal>
            </td>
            <td class="list">
                 <asp:Label ID="lblEqu" runat="server" Visible="False"></asp:Label>
                <asp:TextBox ID="txtEqu" runat="server" MaxLength="80" ></asp:TextBox>
                <input id="cmdEqu" onclick="SelectEqu(this)" type="button" value="..." runat="server"
                    name="cmdEqu" class="btnClass2" />
               
                &nbsp;
            </td>            
           
        </tr>
    </table>
    <table width="98%" class="listNewContent">
        <tr>
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img1" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />
                            展示数据
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="98%" cellpadding="0" border="0">
        <tr id="tr1" runat="server" >
            <td align="left">
                <table id="tblResult" runat="server" cellspacing="0" cellpadding="0" width="100%"
                    border="0">
                    <tr>
                        <td width="100%" align="left" class="listContent" valign="top">                            
                            <asp:DataGrid ID="dgSchemeRatio" runat="server" ShowHeader="true" AutoGenerateColumns="False"
                                GridLines="Vertical" Width="100%">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="资产">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.equipmentname")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="可用率">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.Asse").ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                        
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
</asp:Content>
