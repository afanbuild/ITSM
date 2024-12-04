<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frm_Services_Template.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frm_Services_Template"
    Title="�ޱ���ҳ" %>

<%@ Register Src="../Controls/UserPicker.ascx" TagName="UserPicker" TagPrefix="uc8" %>
<%@ Register Src="../Controls/ServiceStaff.ascx" TagName="ServiceStaff" TagPrefix="uc7" %>
<%@ Register Src="../Controls/ctrFlowCataDropList.ascx" TagName="ctrFlowCataDropList"
    TagPrefix="uc6" %>
<%@ Register Src="../Controls/common/DictionaryPicker.ascx" TagName="DictionaryPicker"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Register TagPrefix="uc11" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Register Src="../Controls/CtrFlowFormText.ascx" TagName="CtrFlowFormText" TagPrefix="uc1" %>
<%@ Register Src="../Controls/browsepic.ascx" TagName="browsepic" TagPrefix="uc3" %>
<%@ Register src="../Controls/ctrattachment.ascx" tagname="ctrattachment" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../Js/jquery-1.7.2.min.js"></script>

    <script language="javascript" src="../Js/jShowDiv.js"></script>

    <script language="javascript" src="../Js/jUtility.js"></script>

    <script language="javascript">
//ҳ�����ִ�л�ȡͼƬ
 $(document).ready(function () {
        var slngid = GetQueryString("subjectid");		                            
        showPic(slngid);     // ��ȡͼƬ��Ϣ     
 });
 
// ��ȡͼƬ��Ϣ
function showPic(lngid) {
   $.ajax({
       type: 'POST',
       url: '../Forms/Handler.ashx',
       data: 'act=easerviceimg&lngid=' + lngid,
       timeout: '10000',
       error: function () {
           alert("��ȡͼƬ��Ϣʧ�ܣ�");
       }, success: function (json) {
           piclistCallBack(json);
       }
   });
}
                                 
//���񼶱�ѡ��
function ServiceLevelSelect(obj) 
{
    var ServiceTypeID = "0";
    var ServiceKindID = "0";
    var ServiceEffID = "0";
    var ServiceInsID = "0";  
    var CustID = "";
    var EquID = ""; 
         
	var	value=window.showModalDialog("frmCst_ServicLevelSelect.aspx?IsSelect=true&randomid="+GetRandom()+"&CustID=" + CustID + "&EquID="+ EquID + "&TypeID=" + ServiceTypeID + "&KindID=" + ServiceKindID + "&EffID=" + ServiceEffID + "&InsID=" + ServiceInsID,window,"dialogHeight:600px;dialogWidth:800px");
	if(value != null)
	{
		if(value.length>1)
		{                         
            document.getElementById(obj.id.replace("cmdPopServiceLevel","txtServiceLevel")).value = value[2];   //��������        
            document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevel")).value = value[2];   //��������
            document.getElementById(obj.id.replace("cmdPopServiceLevel","hidServiceLevelID")).value = value[1];  //����ID                            
		}
	}
}        

//�����¼�
function SubmitValidate()
{
    var objFlag=document.getElementById("<%=radioParent.ClientID %>");//��־
    var objParentList=document.getElementById("<%=drParentList.ClientID %>");//����Ŀ¼
    var objFlowModel=document.getElementById("<%=ddlFlowModel.ClientID %>");//�¼�ģ��
    
    var arr = objFlag.getElementsByTagName("input");
    
    var objvalue = 0;
    if(objFlag!=null){
        var i;
        for(i=0;i<arr.length;i++){
            if(arr[i].checked){
                objvalue= arr[i].value; 
            }
        }
    }
    
    if(objvalue=="0"){
        var pIndex=objParentList.selectedIndex;//����Ŀ¼
        var fIndex=objFlowModel.selectedIndex;//�¼�ģ��
        if(pIndex=="0")
        {
            objParentList.focus();
            alert("����Ŀ¼����Ϊ��!");
            return false;
        }
        if(fIndex=="0")
        {
            objFlowModel.focus();
            alert("�¼�ģ�岻��Ϊ��!");
            return false;
        }
    }
    return true;
}

//��ѯģ��
function ShowIssTemp()
{
    //��ȡ��ǰѡ����¼�ģ��ID
    var obj = document.getElementById("<%=ddlFlowModel.ClientID %>");
    if(obj.options.length<=0)
        return;
    
    if(obj.options[obj.selectedIndex].value=="")
    {
        return;
    }
    
    var tid = obj.options[obj.selectedIndex].value;
    window.open("frm_Issue_Template.aspx?id="+tid+"&IsShow=true","","scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
    event.returnValue = false;
}

//��ѡ��ť�����¼�


function radioClick() {
    var radio=document.all.<%=radioParent.ClientID %>;//��ȡ��ѡ����


    var childName=radio.firstChild.firstChild.firstChild.firstChild.name;
    var childNodes=document.getElementsByName(childName);
    for(var i=0;i<childNodes.length;i++)
    {
        var radValue=childNodes[i].value;
        if(childNodes[i].checked)
        {
            if(radValue=="0"){
               var objFlowModel = document.getElementById("<%=ddlFlowModel.ClientID %>");//�¼�ģ��
               var objGuide=document.getElementById("<%=txtGuide.ClientID %>");//ָ��
               var objHGuide=document.getElementById("<%=hfGuide.ClientID %>");//ָ��
               var objParentList=document.getElementById("<%=drParentList.ClientID %>");//����Ŀ¼
               var objFlag=document.getElementById("<%=hfFlag.ClientID %>");//��־
               objGuide.value=objHGuide.value;//ָ��
               objFlowModel.disabled=false;
               objGuide.disabled=false;
               objParentList.disabled=false;
               objFlag.value="1";
            }else{
               var objFlowModel = document.getElementById("<%=ddlFlowModel.ClientID %>");//�¼�ģ��
               var objGuide=document.getElementById("<%=txtGuide.ClientID %>");//ָ��
               var objParentList=document.getElementById("<%=drParentList.ClientID %>");//����Ŀ¼
               var objFlag=document.getElementById("<%=hfFlag.ClientID %>");//��־
               objGuide.value="";
               objFlowModel.selectedIndex=0;
               objParentList.selectedIndex=0;
               objFlag.value="0";
               objFlowModel.disabled=true;
               objGuide.disabled=true;
               objParentList.disabled=true;
            }
        }
    }
}   

		function delete_confirm()
			{
				if (event.srcElement.value =="ɾ��" )
					event.returnValue =confirm("ȷ��Ҫɾ����?");
			}
		function SelectPCatalog()
			{ 
				if(	document.getElementById('<%=hidCatalogID1.ClientID %>').value== 1)//
				{
					alert("�Ѿ���������!");
					return;
				}
				var newDateObj = new Date();
				var sparamvalue =  newDateObj.getYear().toString() + newDateObj.getMonth().toString() + newDateObj.getMinutes().toString() + newDateObj.getMilliseconds().toString();
				//=========zxl======
				var url="frmpopSubject.aspx?CurrSubjectID=" + document.getElementById('<%=hidCatalogID1.ClientID %>').value + "&paramvalue=" + sparamvalue+"&Opener_ClientId=<%=hidClientId_ForOpenerPage.ClientID %>";
				window.open(url,"","scrollbars=no,status=yes ,resizable=yes,width=800,height=600");
			}
    </script>
    <asp:HiddenField ID="hfRadio" runat="server" Value="0" />
    <asp:HiddenField ID="hfFlag" runat="server" Value="0" />
    <asp:HiddenField ID="hfGuide" runat="server" Value="" />
    <asp:HiddenField ID="hidModified" runat="server" Value="0" />
    <asp:HiddenField ID="hidCatalogID1" runat="server" Value="0" />
        <table style="width: 98%" align="center" class="listContent">
        <tr>
            <td colspan="2" class="list">
                <uc11:CtrTitle ID="CtrTitle" runat="server" Title="����Ŀ¼���� "></uc11:CtrTitle>
            </td>
        </tr>
        <tr height="40">
            <td colspan="2" class="listTitle">
                <asp:Button ID="cmdAdd" runat="server" Text="����" CssClass="btnClass" OnClick="cmdAdd_Click"
                    CausesValidation="false"></asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="����" CssClass="btnClass" OnClick="cmdSave_Click">
                </asp:Button>&nbsp;&nbsp;
                <asp:Button ID="cmdDelete" runat="server" Text="ɾ��" CssClass="btnClass" OnClick="cmdDelete_Click"
                    OnClientClick="delete_confirm();" CausesValidation="false"></asp:Button>&nbsp;
            </td>
        </tr>
    </table>
    <table class="listContent" width="98%" align="center" runat="server" id="Table2">
        <tr>
          <td class="listTitleRight " nowrap="nowrap" style="width: 12%;display:none" >
                �Ƿ�һ��Ŀ¼


            </td>
            <td class="list" style="width: 35%;display:none">
                <asp:RadioButtonList ID="radioParent"  runat="server" 
                    RepeatColumns="2">
                    <asp:ListItem Value="1">��</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">��</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                ����������
            </td>
            <td class="list">
                <uc1:CtrFlowFormText ID="CtrFTTemplateName" runat="server" MaxLength="50" MustInput="true" TextToolTip="����������" />
            </td>
               <td class="listTitleRight " nowrap="nowrap">
                <asp:Literal ID="Literal1" runat="server" Text="�ϼ�Ŀ¼"></asp:Literal>
            </td>
            <td class="list">
                <asp:DropDownList ID="drParentList" runat="server" Width="152px"  Visible=false>
                </asp:DropDownList>
                    <%--===zxl==--%>
                    
        <input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage"
        type="hidden" />
                     <asp:TextBox ID="txtPCatalogName" runat="server" Width="258" ></asp:TextBox>
                         <asp:HiddenField ID="hidPCatalogID" runat="server" Value="0" />      
                            <asp:HiddenField ID="hidPCatalogID1" runat="server" Value="0" />                          
                         <asp:HiddenField ID="hidPCatalogName" runat="server" Value="" />
                    
                <input
                        id="cmdPopParentCatalog" onclick="SelectPCatalog()" type="button" value="..."
                        class="btnClass2">
                 <span style="color:Red; font-size:Small">*</span>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                ����Ӧ��
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="ddlApp" runat="server"  Width="306px" 
                    onselectedindexchanged="ddlApp_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">--��ѡ��--</asp:ListItem>
                    <asp:ListItem Value="1026">�¼�����</asp:ListItem>
                    <asp:ListItem Value="1062">�������</asp:ListItem>
                </asp:DropDownList><asp:Label ID="rWarning" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight">
                �¼�ģ��
            </td>
            <td class="list" colspan="3">
                <asp:DropDownList ID="ddlFlowModel" runat="server"  Width="306px">
                </asp:DropDownList><asp:Label ID="Label1" runat="server" Style="margin-left:7px;" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
<%--                <a href="#" id="isstemp" onclick="ShowIssTemp();">
                    ģ������</a>--%>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                <asp:Literal ID="LitContent" runat="server" Text="��ϸ����"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <asp:TextBox ID="txtContent" runat="server" Width="95%" MaxLength="500" Rows="3"
                    TextMode="MultiLine"></asp:TextBox><asp:Label ID="labContent" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                <asp:Literal ID="LitGuide" runat="server" Text="ָ��"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <asp:TextBox ID="txtGuide" runat="server" Width="95%" MaxLength="500" Rows="3" TextMode="MultiLine"></asp:TextBox><asp:Label
                    ID="labGuide" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
                <asp:Literal ID="Literal2" runat="server" Text="ͼƬLOGO"></asp:Literal>
            </td>
            <td colspan="3" class="list">
                <uc3:browsepic ID="browsepic1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="listTitleRight " nowrap="nowrap" style="width: 12%">
               ��&nbsp;��

            </td>
            <td colspan="3" class="list" width="88%">                
                <uc4:ctrattachment ID="ctrattachment1" runat="server"  width="98%" />                
            </td>
        </tr>
    </table>      
</asp:Content>
