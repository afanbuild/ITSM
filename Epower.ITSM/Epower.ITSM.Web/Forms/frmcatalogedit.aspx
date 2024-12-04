<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>

<%@ Page Language="c#" Inherits="Epower.ITSM.Web.Forms.frmCatalogEdit" CodeBehind="frmCatalogEdit.aspx.cs" %>

<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc3" %>

<%@ Register src="../Controls/CtrFlowRemark.ascx" tagname="CtrFlowRemark" tagprefix="uc4" %>

<html>
<head id="Head1" runat="server">
    <title>����༭</title>
   
    <link href="../App_Themes/NewOldMainPage/css.css" type="text/css" />

    <script language="javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" src="../Js/App_Common.js"></script>

    <script language="javascript" src="../Js/jquery-1.7.2.min.js"></script>

    <script language="javascript" src="../Js/jShowDiv.js"></script>

    <script language="javascript" src="../Js/jUtility.js"></script>

    <style type="text/css">
        .high 
        {
        	BORDER-RIGHT: #228DC7 0px solid; 
            BORDER-TOP: #228DC7 0px solid; 
            BORDER-LEFT: #228DC7 0px solid; 
            BORDER-BOTTOM: #228DC7 0px solid;
            PADDING-RIGHT: 5px; 
            PADDING-LEFT: 5px; 
            background-image:url(../Images/an1.gif);
           width: 68px; 
           height:23px;
           CURSOR: hand;
        }
    </style>

</head>
<script language="javascript">

        function SelectPCatalog() {
            if (document.all.hidCatalogID.value == 1) {
                alert("�Ѿ���������");
                return;
            }
            //==========zxl==
            
            var url="frmpopCatalog.aspx?CurrCatalogID=" + document.all.hidCatalogID.value+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
            
            window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600,left=150");
        }
        function SelectLeader() {
            var value = window.showModalDialog("frmpopstaff.aspx");
            if (value != null) {
                if (value.length > 1) {
                    arr = value.split("@");
                    document.all.txtLeaderName.value = arr[1];
                    document.all.hidLeaderID.value = arr[0];
                }
            }
        }

        function SelectManager() {
            var value = window.showModalDialog("frmpopstaff.aspx");
            if (value != null) {
                if (value.length > 1) {
                    arr = value.split("@");
                    document.all.txtManagerName.value = arr[1];
                    document.all.hidManagerID.value = arr[0];
                }
            }
        }

        function delete_confirm() {
            event.returnValue = confirm("ȷ��Ҫɾ����?");
        }

        function JoinActor() {
            var features =
				'dialogWidth:380px;' +
				'dialogHeight:500px;' +
				'directories:no; localtion:no; menubar:no; status=no; toolbar=no;scroll:no;resizable=yes';
            //window.showModalDialog('frmJoinActor2.aspx?ActorType=20&ObjectID='+document.all.hidUserID.value,'',features);
            window.showModalDialog('frmJoinActor_Container.aspx?ActorType=10&ObjectID=' + document.all.hidCatalogID.value, '', features);
        }


        //ֻ������������
        function NumberKey() {
            if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 46)) {
                //alert(event.keyCode);
                event.returnValue = false;
            }
        }

        function ChangeCatalogType(CatalogKind) {
            var newOption;
            var dpdCatalogType = document.all.dpdCatalogType;
            while (dpdCatalogType.options.length > 0) { dpdCatalogType.remove(0); }

            switch (CatalogKind) {
                case "0":
                    newOption = document.createElement("OPTION");
                    newOption.value = "0";
                    newOption.text = "���ز���";
                    dpdCatalogType.options.add(newOption);

                    newOption = document.createElement("OPTION");
                    newOption.value = "1";
                    newOption.text = "�ۺϰ칫��";
                    dpdCatalogType.options.add(newOption);

                    newOption = document.createElement("OPTION");
                    newOption.value = "5";
                    newOption.text = "���쵼";
                    dpdCatalogType.options.add(newOption);

                    break;

                case "1":
                    newOption = document.createElement("OPTION");
                    newOption.value = "3";
                    newOption.text = "������λ";
                    dpdCatalogType.options.add(newOption);

                    newOption = document.createElement("OPTION");
                    newOption.value = "4";
                    newOption.text = "ֱ����λ";
                    dpdCatalogType.options.add(newOption);

                    break;
            }
        }
        
        function ShowTable(imgCtrl)
        {
              var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
              var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
              var TableID = imgCtrl.id.replace("Img","Table");              
              var tableCtrl;              
              tableCtrl = document.all.item(TableID);
              if(imgCtrl.src.indexOf("icon_expandall") != -1)
              {
                tableCtrl.style.display ="";
                imgCtrl.src = ImgMinusScr ;
                var temp = document.all.<%=hidTable.ClientID%>.value;
                document.all.<%=hidTable.ClientID%>.value = temp.replace(","+tableCtrl.id,""); 
              }
              else
              {
                tableCtrl.style.display ="none";
                imgCtrl.src = ImgPlusScr ;	
                document.all.<%=hidTable.ClientID%>.value =document.all.<%=hidTable.ClientID%>.value + "," + tableCtrl.id;
              }
        }
        
        function doAddNewItem()
	    {
    	    
	        var subjectedId=document.getElementById("hidCatalogID").value;
	        var newDateObj = new Date();	    
	        var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
	        window.open("../Forms/frm_BR_CatalogSchemaItemsMain.aspx?IsChecked=True&IsSelect='1'&subjectedId="+subjectedId+"&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no") ;
	    }
	
	    function AddNewItem()
	    {
	        var newDateObj = new Date();
	        var sparamvalue =  newDateObj.getHours().toString() + newDateObj.getSeconds().toString() + newDateObj.getSeconds().toString() + newDateObj.getMilliseconds().toString();
    	    
	        window.open("../Forms/frm_BR_CatalogSchemaItemsEdit.aspx?IsNew=true&sparamvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>","","dialogWidth=600px; dialogHeight=480px;status=no; help=no;scroll=auto;resizable=no");

	    }
    	
    </script>
<body>
    <form id="Form1" method="post" runat="server">
    <table style="width: 100%" class="listContent">
        <tr>
            <td colspan="2" class="list">
                <uc1:CtrTitle ID="CtrTitle" runat="server"></uc1:CtrTitle>
            </td>
        </tr>
        <tr height="40">
            <td colspan="2" class="listTitle">
                <asp:Button ID="cmdAdd" runat="server" Text="����" CssClass="btnClass" OnClick="cmdAdd_Click">
                </asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="����" CssClass="btnClass" OnClick="cmdSave_Click">
                </asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdDelete" runat="server" Text="ɾ��" CssClass="btnClass" OnClientClick="delete_confirm();"
                    OnClick="cmdDelete_Click"></asp:Button>&nbsp;
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight " style="width: 12%">
                ������:
            </td>
            <td class="list">
                <asp:Label ID="lblCatalogID" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight " style="width: 12%">
                ��������:
            </td>
            <td class="list">
                <asp:TextBox ID="txtCatalogName" runat="server" Width="273px" MaxLength="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight " style="width: 12%">
                ��������:
            </td>
            <td class="list">                
                <uc4:CtrFlowRemark ID="txtDesc" runat="server" Width="273" MaxLength="150" />
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight " style="width: 12%">
                �ϼ�����:
            </td>
            <td class="list">
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
                <asp:TextBox ID="txtPCatalogName" runat="server" Width="258" ReadOnly="True"></asp:TextBox><input
                    id="hidPCatalogID" style="width: 56px" type="hidden" runat="server" name="hidPCatalogID"><input
                        id="cmdPopParentCatalog" onclick="SelectPCatalog()" type="button" value="..."
                        class="btnClass2">
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitleRight " style="width: 12%">
                ��������ID:
            </td>
            <td nowrap class="list">
                <asp:TextBox ID="txtSortID" runat="server" Width="258px" MaxLength="9">-1</asp:TextBox><asp:RangeValidator
                    ID="RangeValidator1" runat="server" ControlToValidate="txtSortID" ErrorMessage="RangeValidator"
                    Type="Integer" MinimumValue="-1" MaximumValue="999999999"> ��������Чֵ</asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td nowrap class="listTitle" style="width: 12%" colspan="3">
                *ע:����IDΪ-1ʱ,��ϵͳָ��������ID,��Чֵ��Χ(-1 ~ 99999)
            </td>
        </tr>
    </table>
    
    <asp:Label ID="labMsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    
    <table id="Table12" width="98%" align="center" runat="server">
        <tr id="tr1" runat="server">
            <td valign="top" align="left" class="listTitleNew">
                <table width="150" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="24" class="bt_di">
                            <img class="icon" id="Img2" onclick="ShowTable(this);" height="16" src="../Images/icon_collapseall.gif"
                                width="16" align="absbottom" />����������
                        </td>
                    </tr>
                </table>
            </td>
            <td class="listTitleNew" align="right" style="width: 160px">       
                <asp:Button ID="btnAddNewItem" runat="server" Text="���������" style="display:none;" CssClass="btnClass" OnClick="btnAddNewItem_Click"
                     CausesValidation="false" />
                <input id="cbtnAdd" class="btnClass" onclick="doAddNewItem();" runat="server" type="button"
                    value="�������" causesvalidation="false" />
                <input id="cbtnNew" class="btnClass" onclick="AddNewItem();" runat="server" type="button"
                    value="��������" causesvalidation="false" />
            </td>
        </tr>
    </table>
    <table style="width: 98%" align="center" runat="server" id="Table2">
        <tr>
            <td>
                <asp:DataGrid ID="dgSchema" runat="server" Width="100%" CellPadding="1" CellSpacing="2"
                    BorderWidth="0px" AutoGenerateColumns="False"  CssClass="Gridtable"  OnItemCommand="dgPro_ProvideManage_ItemCommand"
                    OnItemDataBound="dgPro_ProvideManage_ItemDataBound">
                    <ItemStyle CssClass="tablebody" BackColor="White"></ItemStyle>
                    <HeaderStyle CssClass="listTitle"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtID" Text='<%# DataBinder.Eval(Container, "DataItem.ID")%>' onblur="CheckDoubleID(this,'txtID');"
                                    Width="85%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="���">
                            <ItemTemplate>
                                &nbsp;<asp:DropDownList ID="ddlTypeName" runat="server" onchange="CheckDefaultControlStatus(this);"
                                    Width="95%" SelectedValue='<%# DataBinder.Eval(Container, "DataItem.TypeName") %>'
                                    Enabled="False">
                                    <asp:ListItem>������Ϣ</asp:ListItem>
                                    <asp:ListItem>��������</asp:ListItem>
                                    <asp:ListItem>��ע��Ϣ</asp:ListItem>
                                    <asp:ListItem>����ѡ��</asp:ListItem>
                                    <asp:ListItem>������Ϣ</asp:ListItem>
                                    <asp:ListItem>�û���Ϣ</asp:ListItem>
                                    <asp:ListItem>��������</asp:ListItem>
                                    <asp:ListItem>��ֵ����</asp:ListItem>
                                    <asp:ListItem>��ѡ����</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="����������">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHName" Text='<%# DataBinder.Eval(Container, "DataItem.CHName")%>'
                                    Width="90%" runat="server" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="��ֵ">
                            <ItemTemplate>
                                <asp:Panel ID="PanDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"0")%>'
                                    runat="server" Height="16px" Width="100px">
                                    <asp:CheckBox ID="chkDefault" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" /></asp:Panel>
                                <asp:Panel ID="PantxtDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"1")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:TextBox ID="txtDefault" Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>'
                                        Width="100%" runat="server"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="PantxtMDefault" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"2")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:TextBox ID="txtMDefault" Text='<%# DataBinder.Eval(Container, "DataItem.Default")%>'
                                        Width="100%" runat="server" TextMode="MultiLine"></asp:TextBox></asp:Panel>
                                <asp:Panel ID="panDropDownList" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"3")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <uc2:ctrFlowCataDropList ID="ctrFlowCataDropDefault" runat="server" RootID="1" />
                                </asp:Panel>
                                <asp:Panel ID="panDept" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"4")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckDept" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" />���ڲ���</asp:Panel>
                                <asp:Panel ID="panUser" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"5")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckUser" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" />��¼��</asp:Panel>
                                <asp:Panel ID="PanTime" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"6")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <asp:CheckBox ID="CheckTime" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.Default"))%>'
                                        runat="server" />��ǰ����
                                    <asp:CheckBox ID="CheckIsTime" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.isChack"))%>'
                                        runat="server" />�Ƿ�ʱ��</asp:Panel>
                                <asp:Panel ID="PanNumber" Style='<%# GetDefaulControlState((string)DataBinder.Eval(Container, "DataItem.TypeName"),"7")%>'
                                    runat="server" Height="24px" Width="100px">
                                    <uc3:CtrFlowNumeric ID="TextNumber" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.Default")%>' />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="����">
                            <HeaderStyle Width="65px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsMust" Checked='<%# GetDefaulCheckValue((string)DataBinder.Eval(Container, "DataItem.IsMust"))%>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="��">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGroup" Text='<%# DataBinder.Eval(Container, "DataItem.Group")%>'
                                    Width="95%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="����">
                            <HeaderStyle Width="65px" />
                            <ItemTemplate>
                                <asp:TextBox ID="TxtOrderBy" Text='<%# DataBinder.Eval(Container, "DataItem.OrderBy")%>'
                                    Width="95%" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="ɾ��">
                            <ItemTemplate>
                                <asp:Button ID="lnkdelete"  SkinID="btnClass1" runat="server" Text="ɾ��" CommandName="delete"
                                    CausesValidation="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Default" HeaderText="��ֵ" Visible="false"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    
    <input id="hidIsShowSchema" type="hidden" runat="server" name="hidIsShowSchema" value="0" />
    <input id="hidOrgID" type="hidden" runat="server" name="hidOrgID" />
    <input id="hidCatalogID" type="hidden" runat="server" name="hidCatalogID" />    
    <input id="hidTable" value="" runat="server" type="hidden" />
    <input id="hidTempID" runat="server" type="hidden" />
    <input id="hidSchemaXml" type="hidden" runat="server" name="hidSchemaXml" />
    
    </form>

    <script>
    </script>
     <script type="text/jscript">
        $(document).ready(function() 
        {
          $("#cmdAdd").attr("class", "high");
          $("#cmdSave").attr("class", "high");
          $("#cmdDelete").attr("class", "high"); 
        });
        var temp = document.all.<%=hidTable.ClientID%>.value;
        if(temp!="")
        {
            var ImgPlusScr ="../Images/icon_expandall.gif"	;      	// pic Plus  +
            var ImgMinusScr ="../Images/icon_collapseall.gif"	;	    // pic Minus - 
            var arr=temp.split(",");
            for(i=1;i<arr.length;i++)
            {   
                var tableid=arr[i];
                var tableCtrl = document.all.item(tableid);
                tableCtrl.style.display ="none";
                var ImgID = tableid.replace("Table","Img");
                var imgCtrl = document.all.item(ImgID)
                imgCtrl.src = ImgPlusScr ;	
            }
        }
    
    </script>

</body>
</html>
