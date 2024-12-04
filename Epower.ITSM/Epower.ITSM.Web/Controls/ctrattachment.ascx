<%@ Control Language="c#" Inherits="Epower.ITSM.Web.Controls.CtrAttachment" CodeBehind="CtrAttachment.ascx.cs" %>
<script type="text/javascript" language="javascript" src="../js/uploadify/jquery.uploadify.js"></script> 
<script language="javascript">
    function UploadInit()
        {
            $("#uploadify").uploadify({
                'overrideEvents': ['onDialogClose'],
                'swf': '../js/uploadify/uploadify.swf',
                'uploader': '../js/uploadify/UploadHandler.ashx',
                'cancelImage': '../js/uploadify/uploadify-cancel.png',
                'queueID': 'DivUpload',
                'buttonText': '��Ӹ���',
                'fileSizeLimit': '<%=strMaxFileSize %>MB',
                'onSelectError': function(file, errorCode, errorMsg) {
                    switch (errorCode) {
                        case -100:
                            alert("�ϴ����ļ������Ѿ�����ϵͳ���Ƶ�" + $('#uploadify').uploadify('settings', 'uploadLimit') + "���ļ���");
                            break;
                        case -110:
                            alert("�ļ� [" + file.name + "] ��С����ϵͳ���Ƶ�" + $('#uploadify').uploadify('settings', 'fileSizeLimit') + "��С��");
                            break;
                        case -120:
                            alert("�����ϴ����ļ���");
                            break;
                    }

                },
                'onUploadSuccess': function(file, data, response) {
                    document.getElementById("<%=hidbackData.ClientID %>").value = document.getElementById("<%=hidbackData.ClientID %>").value + "*" + data;
                },
                'onQueueComplete': function(queueData) {                    
                    document.getElementById("<%=cmdAttach.ClientID %>").click();
                },
                'onSelect': function(fileObj) {
                                    
                    if (!fileObj.type) {
                        $("#uploadify").uploadify('cancel', fileObj.id);
                        alert('�����ϴ�û�к�׺�����ļ�!');
                    } else {
                        if (fileObj.type.length > 10) {
                            $("#uploadify").uploadify( 'cancel', fileObj.id );
                            alert('��׺�����Ȳ��ܴ���10���ַ�!');
                        }
                    }
                },
                'auto': true,
                'multi': true
            });
        }
        
	function FileSumit(obj)
	{
		reg=/File1/g;
		var idtag=obj.id.replace(reg,"");
		
		document.all(idtag+"DivChen").style.display="block";
		document.all(idtag+"txtFile").value=obj.value;	
		document.all(idtag+"cmdAttach").click();
		//document.all("DivChen").style.display="none";
}
function FileSumit2(obj) {
    reg = /File2/g;
    var idtag = obj.id.replace(reg, "");
    document.all(idtag+"DivChen").style.display="block";
    document.all(idtag + "btncmdAttach2").click();
}
	function delete_confirmAttachment(obj)     //ɾ��ǰִ�нű�
	{	   
	    if(obj.defaultValue=="ȡ��") 
	    {
	        event.returnValue =confirm("��ȷ��Ҫȡ��������?");
	    }
	    else
	    {
	        event.returnValue =confirm("��ȷ��Ҫɾ����?");
	    }
	    
	}
	function Edit_confirmAttachment() {
	    event.returnValue = confirm("��ȷ��Ҫ������?");
	}
	    
</script>

<style type="text/css">
    div#divZong
    {
        position: relative;
        z-index: 1;
    }
   
</style>

<div id="divZong" style="width: 100%">
    <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0" id="tabMain"
        runat="server">
        <tr>
            <td>
                <table width="100%">
                    <tr valign="top">
                        <td colspan="2" width="100%">
                            <asp:DataGrid ID="dgAttachment" runat="server" PageSize="16" Width="100%" OnItemDataBound="dgAttachment_ItemDataBound"
                                OnItemCommand="dgAttachment_ItemCommand" CssClass="gridTable">
                                <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="�ļ�����" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileName" runat="server" Text='<%#GetUrl((string)DataBinder.Eval(Container.DataItem, "FileName"),(string)DataBinder.Eval(Container.DataItem, "Status"),(string)DataBinder.Eval(Container.DataItem, "IDAndName"))%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" HorizontalAlign="center"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="upTime" HeaderText="�ϴ�ʱ��" ItemStyle-HorizontalAlign="center">
                                        <HeaderStyle Width="20%" HorizontalAlign="center"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="upUserName" ReadOnly="True" HeaderText="�ϴ���" ItemStyle-HorizontalAlign="center">
                                        <HeaderStyle Width="20%" HorizontalAlign="center"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn ItemStyle-Width="13%" HeaderStyle-Width="13%" ItemStyle-HorizontalAlign="center">
                                        <EditItemTemplate>
                                            &nbsp;
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="cmdDel" runat="server" Text="ɾ��" SkinID="btnClass1" OnClientClick="delete_confirmAttachment(this);"
                                                CommandName="delete" />
                                            <asp:Button ID="cmdEdit" runat="server" Text="����" SkinID="btnClass1" OnClientClick="Edit_confirmAttachment();"
                                                CommandName="Edit" />
                                            <asp:Button ID="cmdPreview" runat="server" Text="Ԥ��" SkinID="btnClass1"
                                                CommandName="Preview"  CommandArgument='<%#Eval("IDAndName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="Status" ReadOnly="True"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FileID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FileName" ReadOnly="True" HeaderText="�ļ�����">
                                        <HeaderStyle Width="180px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="requestFileId" ReadOnly="True" HeaderText="�ļ�����">
                                    </asp:BoundColumn>
                                    <%--#region ��ӵĴ���� 
								//******************************************************************************* 
								//* �� �� ���� DW��Ѷ����
								//* �� �� �ˣ� ��ӱ��(chenyingpeng@d-wolves.com)
								//* ������ڣ� 2010��06��25��
								//*******************************************************************************--%>
                                    <asp:BoundColumn DataField="upUserID" HeaderText="�û�ID" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="IsUpdate" Visible="false"></asp:BoundColumn>
                                    <%--#endregion--%>
                                </Columns>
                                <PagerStyle NextPageText="��һҳ" PrevPageText="��һҳ" HorizontalAlign="Left" ForeColor="#000066"
                                    BackColor="White"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr valign="bottom" height="100%">
                        <td>
                            <table width="100%" id="tryinchang" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDelete" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="AttachmentDelete" runat="server" PageSize="16" Width="100%" CssClass="GridTable"
                                            OnItemDataBound="AttachmentDelete_ItemDataBound">
                                            <HeaderStyle CssClass="listTitle" Font-Bold="true" Font-Size="X-Small" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="�ļ�����" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Text='<%#GetUrl((string)DataBinder.Eval(Container.DataItem, "FileName"),(string)DataBinder.Eval(Container.DataItem, "IDAndName"))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30%" HorizontalAlign="center"></HeaderStyle>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="upTime" HeaderText="�ϴ�ʱ��" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle Width="20%" HorizontalAlign="center"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="upUserName" ReadOnly="True" HeaderText="ɾ����" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle Width="10%" HorizontalAlign="center"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="deleteTime" HeaderText="ɾ��ʱ��" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle Width="10%" HorizontalAlign="center"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="IsUpdate" Visible="false"></asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="��һҳ" PrevPageText="��һҳ" HorizontalAlign="Left" ForeColor="#000066"
                                                BackColor="White"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trAdd" runat="server">
                        <td>
                            <table class="listContent" width="100%" border="0" cellspacing="1" cellpadding="1">
                                <tr>
                                    <td  class="list">
                                        <input id="txtFile"  type="text" size="57" runat="server" readonly="true"  >
                                    </td>
                                    <td class="list" align="center" style="padding-top:11px;">
                                        <input type="file" name="uploadify" id="uploadify"/>
                                        <%--<input id="File1" type="file" style="width: 70px; height: 23px;" size="0" 
                                            onchange="FileSumit(this);" runat="server">--%>
                                           <asp:Button ID="cmdAttach" Style="visibility: hidden" runat="server" Width="0px"
                                            Text="���" Height="0px" OnClick="cmdAttach_Click"></asp:Button>
                                      
                                    </td>
                                    <td class="list" style="width: 100px; display: none;">
                                        <asp:Button ID="ButXS" runat="server" Text="��ʾ��ʷ����" Visible="true" OnClick="ButXS_Click"
                                            SkinID="btnClass3" />
                                        <asp:Button ID="ButYC" runat="server" Text="������ʷ����" Visible="false" OnClick="ButYC_Click"
                                            SkinID="btnClass3" />
                                            
                                           
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="bottom" height="100%" id="trRemark" runat="server" style="display:none;">
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <font color="#0000ff">* �����������ɶ���ϴ��ļ�</font>
                        </td>
                        <td align="right">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="updateFileDiv" style="position: absolute; background-color: #cde3f6; z-index: 101;
        width: 40%; height: 100px; text-align: center; border: 1px solid bule; top: 10%;
        left: 25%" runat="server" visible="false">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" cssclass="listContent_1">
            <tr>
                <td align="center" class="list">
                    <font size="3"><b>��������</b></font></br> </br>
                    <input id="File2" class="btnClass" type="file" onchange="FileSumit2(this);" size="0"
                        runat="server">
                    
                     <asp:Button ID="btncmdAttach2" Style="visibility: hidden" runat="server"
                            Width="0px" Text="���" Height="0px" OnClick="btncmdAttach2_Click"></asp:Button>
                    <asp:Button ID="Button1" runat="server" Text="ȡ��" OnClick="Button1_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
    <div id="DivChen" style="z-index: 102; width: 20%; height: 30px; background-color: #cde3f6;
        margin: auto; text-align: center; border: 1px solid bule; position: absolute;
        top: 15%; left: 35%; display: none" runat="server">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" cssclass="listContent_1">
            <tr>
                <td align="center">
                    �����ϴ��ļ������������Ժ�
                </td>
            </tr>
        </table>
    </div>
    <div id="DivUpload" style="z-index: 102;margin: auto; text-align: center; position: absolute;
        top: 15%; left: 35%;">        
    </div>
    <input id="deleteFileId" type="hidden" runat="server" />
    <asp:Label ID="labMsg" runat="server" Visible="False"></asp:Label>
    <input id="hidMaxFileSize" type="hidden" runat="server" value="4" />
    <input id="hidbackData" type="hidden" runat="server" value="" />
</div>
