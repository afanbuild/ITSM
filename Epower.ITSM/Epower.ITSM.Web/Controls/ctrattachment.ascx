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
                'buttonText': '添加附件',
                'fileSizeLimit': '<%=strMaxFileSize %>MB',
                'onSelectError': function(file, errorCode, errorMsg) {
                    switch (errorCode) {
                        case -100:
                            alert("上传的文件数量已经超出系统限制的" + $('#uploadify').uploadify('settings', 'uploadLimit') + "个文件！");
                            break;
                        case -110:
                            alert("文件 [" + file.name + "] 大小超出系统限制的" + $('#uploadify').uploadify('settings', 'fileSizeLimit') + "大小！");
                            break;
                        case -120:
                            alert("不能上传空文件！");
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
                        alert('不能上传没有后缀名的文件!');
                    } else {
                        if (fileObj.type.length > 10) {
                            $("#uploadify").uploadify( 'cancel', fileObj.id );
                            alert('后缀名长度不能大于10个字符!');
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
	function delete_confirmAttachment(obj)     //删除前执行脚本
	{	   
	    if(obj.defaultValue=="取消") 
	    {
	        event.returnValue =confirm("您确认要取消更新吗?");
	    }
	    else
	    {
	        event.returnValue =confirm("您确认要删除吗?");
	    }
	    
	}
	function Edit_confirmAttachment() {
	    event.returnValue = confirm("您确认要更新吗?");
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
                                    <asp:TemplateColumn HeaderText="文件名称" ItemStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileName" runat="server" Text='<%#GetUrl((string)DataBinder.Eval(Container.DataItem, "FileName"),(string)DataBinder.Eval(Container.DataItem, "Status"),(string)DataBinder.Eval(Container.DataItem, "IDAndName"))%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="30%" HorizontalAlign="center"></HeaderStyle>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="upTime" HeaderText="上传时间" ItemStyle-HorizontalAlign="center">
                                        <HeaderStyle Width="20%" HorizontalAlign="center"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="upUserName" ReadOnly="True" HeaderText="上传人" ItemStyle-HorizontalAlign="center">
                                        <HeaderStyle Width="20%" HorizontalAlign="center"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn ItemStyle-Width="13%" HeaderStyle-Width="13%" ItemStyle-HorizontalAlign="center">
                                        <EditItemTemplate>
                                            &nbsp;
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="cmdDel" runat="server" Text="删除" SkinID="btnClass1" OnClientClick="delete_confirmAttachment(this);"
                                                CommandName="delete" />
                                            <asp:Button ID="cmdEdit" runat="server" Text="更新" SkinID="btnClass1" OnClientClick="Edit_confirmAttachment();"
                                                CommandName="Edit" />
                                            <asp:Button ID="cmdPreview" runat="server" Text="预览" SkinID="btnClass1"
                                                CommandName="Preview"  CommandArgument='<%#Eval("IDAndName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="Status" ReadOnly="True"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FileID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="FileName" ReadOnly="True" HeaderText="文件名称">
                                        <HeaderStyle Width="180px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="requestFileId" ReadOnly="True" HeaderText="文件名称">
                                    </asp:BoundColumn>
                                    <%--#region 添加的代码段 
								//******************************************************************************* 
								//* 添 加 方： DW资讯中心
								//* 添 加 人： 陈颖鹏(chenyingpeng@d-wolves.com)
								//* 添加日期： 2010年06月25日
								//*******************************************************************************--%>
                                    <asp:BoundColumn DataField="upUserID" HeaderText="用户ID" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="IsUpdate" Visible="false"></asp:BoundColumn>
                                    <%--#endregion--%>
                                </Columns>
                                <PagerStyle NextPageText="下一页" PrevPageText="上一页" HorizontalAlign="Left" ForeColor="#000066"
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
                                                <asp:TemplateColumn HeaderText="文件名称" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Text='<%#GetUrl((string)DataBinder.Eval(Container.DataItem, "FileName"),(string)DataBinder.Eval(Container.DataItem, "IDAndName"))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30%" HorizontalAlign="center"></HeaderStyle>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="upTime" HeaderText="上传时间" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle Width="20%" HorizontalAlign="center"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="upUserName" ReadOnly="True" HeaderText="删除人" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle Width="10%" HorizontalAlign="center"></HeaderStyle>
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="deleteTime" HeaderText="删除时间" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle Width="10%" HorizontalAlign="center"></HeaderStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="IsUpdate" Visible="false"></asp:BoundColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="下一页" PrevPageText="上一页" HorizontalAlign="Left" ForeColor="#000066"
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
                                            Text="添加" Height="0px" OnClick="cmdAttach_Click"></asp:Button>
                                      
                                    </td>
                                    <td class="list" style="width: 100px; display: none;">
                                        <asp:Button ID="ButXS" runat="server" Text="显示历史附件" Visible="true" OnClick="ButXS_Click"
                                            SkinID="btnClass3" />
                                        <asp:Button ID="ButYC" runat="server" Text="隐藏历史附件" Visible="false" OnClick="ButYC_Click"
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
                            <font color="#0000ff">* 点击“浏览”可多次上传文件</font>
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
                    <font size="3"><b>附件更新</b></font></br> </br>
                    <input id="File2" class="btnClass" type="file" onchange="FileSumit2(this);" size="0"
                        runat="server">
                    
                     <asp:Button ID="btncmdAttach2" Style="visibility: hidden" runat="server"
                            Width="0px" Text="添加" Height="0px" OnClick="btncmdAttach2_Click"></asp:Button>
                    <asp:Button ID="Button1" runat="server" Text="取消" OnClick="Button1_Click"></asp:Button>
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
                    正在上传文件。。。。请稍后
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
