<%@ Page Title="����������ͻ���" Language="C#" MasterPageFile="~/FlowForms.Master" AutoEventWireup="true"
    CodeBehind="frmFlow_QuestHouse.aspx.cs" Inherits="Epower.ITSM.Web.QuestHouse.frmFlow_QuestHouse" ValidateRequest="false" %>

<%@ Register Src="../Controls/CtrDateAndTimeV2.ascx" TagName="ctrdateandtime" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtrFlowRemark.ascx" TagName="CtrFlowRemark" TagPrefix="uc3" %>
<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript">

//�����б�ı�
function onTXJZ(dom)
{
    var valName=document.all.<%=ctrTXJZname.ClientID%>;//��������
    var valType=dom.options[dom.selectedIndex].value;//�Ƿ����
    var valSpn=document.getElementById('spnTXJZ');//��ʾ��
    if(valType=="1")
    {
        valName.style.display="";//��������
        valSpn.style.display="";//��ʾ��
    }else{
        valName.value="";
        valName.style.display="none";//��������
        valSpn.style.display="none";//��ʾ��
    }
}

function TransferValue()
{
   if (typeof(document.all.<%=lb_createDeptname.ClientID%>)!="undefined" )
   {
	   parent.header.flowInfo.Subject.value = document.all.<%=lb_createDeptname.ClientID%>.innerText + document.all.<%=Lb_execByName.ClientID%>.innerText+" [���������] ���뵥";	
   }
}

function getExecUser()
{
   // var url="../mydestop/frmSelectPerson.htm";
   var url="../mydestop/frmperson_zxl.aspx?TypeFrm=QuestHouse&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
    window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=400,height=300,left=150,top=50");
}
        
function DoUserValidate(lngActionID,strActionName)
{
    TransferValue();
    return CheckCustAndType();
}

function GetRandom() {
    return Math.floor(Math.random() * 1000 + 1);
}
    
String.prototype.trim = function()  //ȥ�ո�
{
	// ��������ʽ��ǰ��ո�
	// �ÿ��ַ��������
	return this.replace(/(^\s*)|(\s*$)/g, "").replace("&#160;","");
}	    
//��������ı�
function ChangeOpObj(dom)
{
    var houseId=document.getElementById("<%=HouseID.ClientID%>").value;//�������������ID
    var hidObjId=document.getElementById("hidOpObjId");//��������ID
    var opObjId=dom.options[dom.selectedIndex].value;//��������ID
    hidObjId.value=opObjId;//��������ID
    
    if(xmlhttp == null)
    {
       xmlhttp = CreateXmlHttpObject();   
    }
    if(xmlhttp != null)
    {
        try
        {	
			xmlhttp.open("GET", "../MyDestop/frmXmlHttp.aspx?HouseID=" + escape(houseId)+"&OpObjID="+escape(opObjId)+"&random="+GetRandom(), true); 
            xmlhttp.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
			xmlhttp.onreadystatechange = function() 
			{ 
				if ( xmlhttp.readyState==4 ) 
				{
				    Iframe0.location.reload(); 
				    Iframe1.location.reload(); 
				    Iframe2.location.reload(); 
				} 
			} 
			xmlhttp.send(null); 
		}catch(e3){}
    }
}

//����Ƿ�ѡ�����¼���Դ���¼�����
function CheckCustAndType()
{
    var txtName=document.all.<%=ctrTXJZname.ClientID%>;//��������
    
    
    if(typeof(txtName)!="undefined")
    {
        var ddlTXJZ=document.all.<%=ddlTXJZ.ClientID %>;//�Ƿ����
        var valType=ddlTXJZ.options[ddlTXJZ.selectedIndex].value;
        if(valType=="1"&&txtName.value.trim()=="")
        {
           txtName.focus();
           alert("�������Ʋ���Ϊ�գ�");
           return false;
        }
    }
    
    var cblAddress=document.all.<%=cblAddress.ClientID %>;//�����˽����ַ
    if(typeof(cblAddress)!="undefined")
    {
        if(cblAddress.disabled==false)
        {
            var   str   ="";
            
            for(i=0;i<cblAddress.rows.length;i++)
            {    
                for(j=0;j<cblAddress.rows[i].cells.length;j++)
                {
                    if(cblAddress.rows[i].cells[j].childNodes[0])
                    {    
                        if(cblAddress.rows[i].cells[j].childNodes[0].checked==true)
                            str+=cblAddress.rows[i].cells[j].childNodes[1].innerText+"    ";
                    }
                }
            }
            if(str== "")
            { 
                alert("�����˽����ַ��ѡ��");
                return false;
            }
        }
    }
    return true;
}

    </script>

    <input type="hidden" value="0" id="hidOpObjId" />
    <input id="chkSelObject" type="hidden" value="false" />
    <input type="hidden" runat="server" id="HouseID" />
     <input id="hidFlowID" type="hidden" runat="server" value="0" />
    <input id="hidActorClass" type="hidden" runat="server" value="-1" />
    
    <table class="listContent" width="100%" align="center" runat="server" id="Table2">
        <tr>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_ITILNO" runat="server" Text="ITIL��"></asp:Literal>
            </td>
            <td class="list" style="width: 35%">
                <asp:Label ID="Lb_ITILNO" runat="server" Text=""></asp:Label>
            </td>
            <td style="width: 12%" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_IsBudan" runat="server" Text="������־"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddl_IsBudan" runat="server">
                    <asp:ListItem Value="0">��</asp:ListItem>
                    <asp:ListItem Value="1">��</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_CreateByName" runat="server" Text="ITIL¼����"></asp:Literal>
            </td>
            <td class="list" >
                <input type="hidden" id="lb_createById" runat="server" />
                <asp:Label ID="lb_createByName" runat="server"></asp:Label>
            </td>
            <td style="width: 130px" class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_CJRByPhone" runat="server" Text="¼���˵绰"></asp:Literal>
            </td>
            <td class="list" >
                <asp:Label ID="Lb_createPhone" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <input type="hidden" id="hd_deptId" runat="server" />
                <asp:Literal ID="Quest_createbydeptname" runat="server" Text="¼���˲���(��)"></asp:Literal>
            </td>
            <td class="list" >
                <asp:Label ID="lb_createDeptname" runat="server"></asp:Label>
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_createdate" runat="server" Text="¼��ʱ��"></asp:Literal>
            </td>
            <td class="list" >
                <asp:Label ID="lb_createDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_comeindate" runat="server" Text="Ԥ�ƽ������ڡ�ʱ��"></asp:Literal>
            </td>
            <td class="list" >
                <uc1:ctrdateandtime ID="ctr_JRdate" runat="server" TextToolTip="Ԥ�ƽ���ʱ��" MustInput="true" />
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_outdate" runat="server" Text="Ԥ���뿪���ڡ�ʱ��"></asp:Literal>
            </td>
            <td class="list" >
                <uc1:ctrdateandtime ID="ctr_OutDate" runat="server" TextToolTip="Ԥ���뿪ʱ��" MustInput="true" />
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_execbyname" runat="server" Text="������"></asp:Literal>
            </td>
            <td class="list" >
                <input id="HidexecById" type="hidden" runat="server" />
                <input id="HidexecByName" type="hidden" runat="server" />
                <asp:Label ID="Lb_execByName" runat="server"></asp:Label>
                <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
                <input id="btn_execByname" type="button" runat="server" value="..." class="btnClass2"
                    onclick="getExecUser();" />
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_execbydeptname" runat="server" Text="�����˲���(��)"></asp:Literal>
            </td>
            <td class="list" >
                <input id="HiddeptId" type="hidden" runat="server" />
                <input id="HiddeptName" type="hidden" runat="server" />
                <asp:Label ID="lb_execDeptname" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_execbyno" runat="server" Text="�����˹��ƺ�"></asp:Literal>
            </td>
            <td class="list" >
                <input id="HidexecByGP" type="hidden" runat="server" />
                <asp:Label ID="lb_execByGH" runat="server"></asp:Label>
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_execbyphone" runat="server" Text="�����˵绰"></asp:Literal>
            </td>
            <td class="list" >
                <input id="HidexecByPhone" type="hidden" runat="server" />
                <asp:Label ID="lb_execByPhone" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_Address" runat="server" Text="�����˽����ַ"></asp:Literal>
            </td>
            <td class="list" >
                <asp:CheckBoxList ID="cblAddress" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                    <asp:ListItem Text="������" Value="1"></asp:ListItem>
                    <asp:ListItem Text="A����" Value="2"></asp:ListItem>
                    <asp:ListItem Text="B����" Value="3"></asp:ListItem>
                    <asp:ListItem Text="C����" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Q����" Value="5"></asp:ListItem>
                    <asp:ListItem Text="���������" Value="6"></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_ActionTypeID" runat="server" Text="��������"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="ddlActionType" runat="server" Width="120px">
                    <asp:ListItem Value="0" Text="�ճ����" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" Text="�ճ�ά��"></asp:ListItem>
                    <asp:ListItem Value="2" Text="���ϴ���"></asp:ListItem>
                    <asp:ListItem Value="3" Text="���Ͷ��"></asp:ListItem>
                    <asp:ListItem Value="4" Text="����"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
       <%-- <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                ��������
            </td>
            <td class="list" >
                <asp:DropDownList ID="ddlOpObj" runat="server" Width="120px" onchange="ChangeOpObj(this)">
                </asp:DropDownList>
                <asp:CheckBox ID="chkSelSave" runat="server" Text="����" 
                    onclick="setHidValue()" />
                <asp:Label ID="lblOpObj" runat="server"  Text="" Visible="false"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_sqdescr" runat="server" Text="��������"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <uc3:CtrFlowRemark ID="CtrFlowRemark1" runat="server" TextToolTip="��������" MustInput="true" />
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_qtexecbyname" runat="server" Text="�������������"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <iframe id='Iframe0' name="Iframe0" src="" width='100%' scrolling='auto' frameborder='no'>
                </iframe>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_hangwuser" runat="server" Text="������Ա"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <iframe id='Iframe1' name="Iframe1" src="" width='100%' height='100' scrolling='auto'
                    frameborder='no'></iframe>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_squserpwd" runat="server" Text="�����û�����"></asp:Literal>
            </td>
            <td class="list"  colspan="3">
                <iframe id='Iframe2' name="Iframe2" src="" width='100%' height='100' scrolling='auto'
                    frameborder='no'></iframe>
            </td>
        </tr>
        <tr>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_txjzis" runat="server" Text="�Ƿ�Я������"></asp:Literal>
            </td>
            <td class="list" >
                <asp:DropDownList ID="ddlTXJZ" runat="server" Width="40px" onchange="onTXJZ(this)">
                    <asp:ListItem Text="��" Value="1"></asp:ListItem>
                    <asp:ListItem Text="��" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <uc2:CtrFlowFormText ID="ctrTXJZname" runat="server" Width="100px" />
                <span style="color: Red;" id="spnTXJZ">Я������ʱֱ�ӵ���������ң�</span>
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_isokscan" runat="server" Text="�Ƿ�ȫɨ��"></asp:Literal>
            </td>
            <td class="list" >
                <uc5:ctrFlowCataDropList ID="ctrFlowIsOkScan" runat="server" RootID="1035" />
            </td>
        </tr>
        <tr style="display: none;">
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_zgflowdate" runat="server" Text="����������(����\ʱ��)"></asp:Literal>
            </td>
            <td class="list" >
                <asp:Label ID="ZGFlowDate" runat="server"></asp:Label>
            </td>
            <td  class="listTitleRight" nowrap="nowrap">
                <asp:Literal ID="Quest_SZSZGflowDate" runat="server" Text="��������������(����\ʱ��)"></asp:Literal>
            </td>
            <td class="list" >
                <asp:Label ID="SZSZGflowDate" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hidPeople" runat="server" />
    <input type="hidden" id="hidJrjf" runat="server" />

    <script type="Text/javascript" language="javascript">
    
        var chkSelObjectvalue= "0";  
        for(var i=0;i <document.all.length;i++) 
        { 
              if(document.all[i].type== 'checkbox') 
              { 
                      if(document.all[i].checked==true) 
                      { 
                              chkSelObjectvalue="1"; 
                      } 
              } 
        } 
    
        Iframe0.location = "../QuestHouse/AddINJFpeoplePage.aspx?HouseID=" + document.getElementById("<%=HouseID.ClientID%>").value + "&showdisplay=" + document.getElementById("<%=hidPeople.ClientID%>").value;
        Iframe1.location = "../QuestHouse/AddExecUserTblPage.aspx?HouseID=" + document.getElementById("<%=HouseID.ClientID%>").value + "&showdisplay=" + document.getElementById("<%=hidPeople.ClientID%>").value;
        Iframe2.location = "../QuestHouse/AddComputerJRJF.aspx?HouseID=" + document.getElementById("<%=HouseID.ClientID%>").value + "&showdisplay=" + document.getElementById("<%=hidJrjf.ClientID%>").value;
        var valName = document.all.<%=ctrTXJZname.ClientID %>; //��������
        var ddlTXJZ=document.all.<%=ddlTXJZ.ClientID %>;//�Ƿ����
        var valType = ddlTXJZ.options[ddlTXJZ.selectedIndex].value; //�Ƿ����
        var valSpn = document.all.spnTXJZ; //��ʾ��
    </script>

</asp:Content>
