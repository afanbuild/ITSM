<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrCondition.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.ctrCondition" %>
<%@ Register Src="CtrFlowNumeric.ascx" TagName="CtrFlowNumeric" TagPrefix="uc2" %>



<script language="javascript" type="text/javascript">

    function DeleteConfim() {
        if (confirm("ȷ��Ҫɾ���˲�ѯ������?")) {
            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }

    function PopCondValue(obj) {
        reg = /cmdPop/g;
        var idtag = obj.id.replace(reg, "");
        //debugger;
        var v = document.all(idtag + "cboItems").value;
        var itemType = v.split(',')[1];
        var itemID = v.split(',')[0];
        var TableName = document.getElementById("<%=hidTableName.ClientID %>").value;

        var url = "../AppForms/frmConditionPop.aspx?Column=" + itemID + "&objID=" + obj.id + "&CondType=" + itemType + "&TableName=" + TableName;
        window.open(url, "", "resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=220,height=250,left=400,top=300");

    }

    //�����ֶεĸı���ıȽϷ�
    function ChangeFieldName(obj) {
        reg = /cboItems/g;
        var idtag = obj.id.replace(reg, "");
        var ddlOperator = document.getElementById(obj.id.replace("cboItems", "cboOperate"));
        while (ddlOperator.options.length > 0) { ddlOperator.options.length = 0; } //��ձȽϷ�

        var SelectField = obj.options[obj.selectedIndex].value;
        var FieldType = SelectField.split(',')[1];
        var newOption;
        //alert(FieldType);
        switch (FieldType) {
            case "CHAR":
                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "0";
                newOption.selected = true;
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "1";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "��..��ͷ";   //���ڵ���
                newOption.value = "2";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "3";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "4";
                ddlOperator.options.add(newOption);

                document.all(idtag + "cmdPop").style.visibility = "Hidden";

                document.all(idtag + "txtValue").disabled = false;

                //�ı��Ժ� ����ط���ֵ���
                document.all(idtag + "txtValue").value = "";
                document.all(idtag + "hidValue").value = "";
                document.all(idtag + "hidTag").value = "";

                break;
            case "CLOB":

                newOption = document.createElement("OPTION");
                newOption.text = "��..��ͷ";   //���ڵ���
                newOption.value = "2";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "3";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "4";
                ddlOperator.options.add(newOption);

                document.all(idtag + "cmdPop").style.visibility = "Hidden";

                document.all(idtag + "txtValue").disabled = false;

                //�ı��Ժ� ����ط���ֵ���
                document.all(idtag + "txtValue").value = "";
                document.all(idtag + "hidValue").value = "";
                document.all(idtag + "hidTag").value = "";

                break;
            case "CATA":
                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "0";
                newOption.selected = true;
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "1";
                ddlOperator.options.add(newOption);


                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "5";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "6";
                ddlOperator.options.add(newOption);

                document.all(idtag + "cmdPop").style.visibility = "Visible";

                //�ı��Ժ� ����ط���ֵ���

                document.all(idtag + "txtValue").disabled = true;

                document.all(idtag + "txtValue").value = "";
                document.all(idtag + "hidValue").value = "";
                document.all(idtag + "hidTag").value = "";

                break;
            case "USER":
                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "0";
                newOption.selected = true;
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "1";
                ddlOperator.options.add(newOption);
                
                
                document.all(idtag + "cmdPop").style.visibility = "Visible";
                
                document.all(idtag + "txtValue").disabled = true;

                //�ı��Ժ� ����ط���ֵ���
                document.all(idtag + "txtValue").value = "";
                document.all(idtag + "hidValue").value = "";
                document.all(idtag + "hidTag").value = "";
                
                break;
            case "DEPT":
                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "0";
                newOption.selected = true;
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "1";
                ddlOperator.options.add(newOption);


                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "5";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "������";
                newOption.value = "6";
                ddlOperator.options.add(newOption);

                document.all(idtag + "cmdPop").style.visibility = "Visible";
                document.all(idtag + "txtValue").disabled = true;
                //�ı��Ժ� ����ط���ֵ���

                document.all(idtag + "txtValue").value = "";
                document.all(idtag + "hidValue").value = "";
                document.all(idtag + "hidTag").value = "";

                break;
            case "DATE":
                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "0";
                newOption.selected = true;
                ddlOperator.options.add(newOption);
                
                newOption = document.createElement("OPTION");
                newOption.text = "����";
                newOption.value = "7";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "���ڵ���";
                newOption.value = "8";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "С��";
                newOption.value = "9";
                ddlOperator.options.add(newOption);

                newOption = document.createElement("OPTION");
                newOption.text = "С�ڵ���";
                newOption.value = "10";
                ddlOperator.options.add(newOption);

                document.all(idtag + "cmdPop").style.visibility = "Visible";
                document.all(idtag + "txtValue").disabled = true;

                //�ı��Ժ� ����ط���ֵ���										

                document.all(idtag + "txtValue").value = "";
                document.all(idtag + "hidValue").value = "";
                document.all(idtag + "hidTag").value = "";

                break;
        }
        ddlOperator.selectedIndex = 0;
        
        
    }
</script>

<style type="text/css">
    .advanced_condition_collapsed_td
    {
        background-color: #E0F8F1;
        border: 1px solid #BDBDBD;
    }
    #advanced_condition_collapsed strong
    {
        text-decoration: underline;
        color: Gray;
    }
    #advanced_condition_collapsed span
    {
        text-decoration: underline;
        font-weight: bold;
    }
    #advanced_condition_collapsed_left
    {
        float: left;
        width: 95%;
        padding-top: 10px;
        padding-bottom: 10px;
        padding-left: 1%;
    }
    #advanced_condition_collapsed_right
    {
    	display:none;
        float: right;
        width: 5%;
        text-align: center;
        border-left: 2px solid gray;               
    }
    #advanced_condition_collapsed_left_keywords
    {
        float: left;
        cursor:pointer;
    }
        
    #advanced_condition_collapsed_left_list
    {
        clear: both;
        float: left;
        margin-top: 10px;
        width: 500px;
    }
    
    #<%=txtConditionName.ClientID%>
    {
    	width: 300px;
    	height:22px;
    	border: 1px solid gray;
    	padding-left:5px;
    	padding-top:2px;
    	padding-bottom:2px;
    	text-align:center;
    }
</style>
<div id="advanced_condition">
    <input id="hidTableName" runat="server" type="hidden" value="" />
    <table style="width: 100%" align="center" runat="server" cellpadding="1" id="TableCondition"
        cellspacing="0" border="0">
        <tr>
            <td class="advanced_condition_collapsed_td" style="width: 100%">
                <div id="advanced_condition_collapsed">
                    <div id="advanced_condition_collapsed_left">
                        <div id="advanced_condition_collapsed_left_keywords">
                            ��ѯ����:&nbsp; 
                            <asp:Literal ID="literalConditionFriendlyContent" runat="server"></asp:Literal>
                        </div>
                        <div id="advanced_condition_collapsed_left_list">
                            <label>
                                �л����:
                            </label>
                            <asp:DropDownList ID="ddlCondition" runat="server">
                            </asp:DropDownList>
                            
                            <span id="delete_query_item" style="text-decoration:none;border-bottom:1px dashed blue; color:Blue; cursor:pointer; display:none;" title="ɾ��ѡ�еĲ�ѯ���"><< ɾ��</span>
                  
                        </div>
                    </div>
                    <div id="advanced_condition_collapsed_right">
                        <img src="../images/advanced_condition_search.png" alt="��ѯ" title="��ѯ" />
                        <img width="42" height="42" src="../images/collapse_arrow_alt.png" />
                    </div>
                </div>
            </td>
        </tr>
        <tr id="trConditionList" style="display:none;" runat="server">
            <td style="width: 100%">
                <div id="advanced_condition_expand">
                    <asp:DataGrid ID="dgCondition" runat="server" AutoGenerateColumns="False" Width="100%"
                        CssClass="fixed-grid-border2" Style="border: 1px solid #A3C9E1;" OnItemDataBound="dgCondition_ItemDataBound"
                        OnItemCommand="dgCondition_ItemCommand">
                        <Columns>
                            <asp:BoundColumn DataField="id" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="��">
                                <ItemTemplate>
                                    <uc2:CtrFlowNumeric ID="CtrFlowGroupValue" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GroupValue") %>'
                                        Width="70px" />
                                </ItemTemplate>
                                <ItemStyle Width="100px" Wrap="True" HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="������">
                                <ItemTemplate>
                                    <asp:DropDownList ID="cboItems" runat="server" onchange="ChangeFieldName(this);"
                                        Width="100">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="�ȽϹ�ϵ">
                                <ItemTemplate>
                                    <asp:DropDownList ID="cboOperate" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Operate") %>'
                                        Width="80" EnableViewState="false">
                                        <asp:ListItem Selected="True" Value="0">����</asp:ListItem>
                                        <asp:ListItem Value="1">������</asp:ListItem>
                                        <asp:ListItem Value="2">��..��ͷ</asp:ListItem>
                                        <asp:ListItem Value="3">����</asp:ListItem>
                                        <asp:ListItem Value="4">������</asp:ListItem>
                                        <asp:ListItem Value="5">����</asp:ListItem>
                                        <asp:ListItem Value="6">������</asp:ListItem>
                                        <asp:ListItem Value="7">����</asp:ListItem>
                                        <asp:ListItem Value="8">���ڵ���</asp:ListItem>
                                        <asp:ListItem Value="9">С��</asp:ListItem>
                                        <asp:ListItem Value="10">С�ڵ���</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="�Ƚ�ֵ">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValue" runat="server" Width="500" Text='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'> 
                                    </asp:TextBox>
                                    <input id="cmdPop" runat="server" class="btnClass2" onclick="PopCondValue(this)"
                                        type="button" style="visibility: hidden" value="...">
                                    <input id="hidValue" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Expression") %>'>
                                    <input id="HidTag" runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Tag") %>'>
                                </ItemTemplate>
                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="�߼���ϵ">
                                <ItemTemplate>
                                    <asp:DropDownList ID="cboRelation" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Relation") %>'
                                        Width="60">
                                        <asp:ListItem Selected="True" Value="0">����</asp:ListItem>
                                        <asp:ListItem Value="1">����</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:Button ID="cmdOK" runat="server" SkinID="btnClass1" OnClick="cmdOK_Click" Text="���"
                                        CausesValidation="False" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btndelete" CommandName="Delete" runat="server" Text="ɾ��" CausesValidation="False"
                                        SkinID="btnClass1"></asp:Button>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="44px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="CondType" Visible="False"></asp:BoundColumn>
                        </Columns>
                        <HeaderStyle CssClass="listTitle" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr id="trConditionButton" style="display:none;" runat="server">
            <td align="center">
            <div id="advanced_condition_expand_part2">
                <asp:DropDownList ID="ddlSelect"  Visible="false" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnDelete" runat="server" Text="ɾ��" Style="display:none;" OnClick="btnDelete_Click" />&nbsp;
                    
                    <div>
                    <input id="txtConditionName" runat="server" value="" type="text" />
                    </div>
                    
         <div style="margin-top:10px; margin-bottom:10px;">
                <asp:Button ID="btnReset" runat="server" Text="ȫ������" OnClick="btnReset_Click" />&nbsp;
                <asp:Button ID="btnSelect" runat="server" Text="��ѯ" />&nbsp;                
                <asp:CheckBox ID="chkHiddenConditionPanel" runat="server" Text="������" />
                <asp:Button ID="btnSave" runat="server"  Visible="false" Text="����" OnClick="btnSave_Click" />&nbsp;             
         </div>   </div>
            </td>
        </tr>
    </table>
    
    <!--Begin: �Ƿ���ʾ�߼���ѯ��� -->
    <!-- Comment: 0 ����, 1 ��ʾ. -->
    <asp:HiddenField ID="hidIsShowAdvancedSearch" EnableViewState="true" Value="0" runat="server" />
    <!--End. -->
</div>

<script type="text/javascript" language="javascript">

$(document).ready(function(){
    try{
    
        // # ��ҳͷ����[�߼���ѯ]��ť
        var search_btn = $('#ctrtabbuttons tr:first > td:eq(1) > input:submit:eq(0)');                  
        if (search_btn.size() > 0) {
      
            search_btn.after('&nbsp;<input type="button" name="btn_advance_query" value="�߼���ѯ" id="btn_advance_query" class="btnClass">');        
            
            search_btn.next().click(function(){                     
                var displayMode = $('#advanced_condition').css('display');
    
                if (displayMode != 'none') {                    
                    displayMode = 'none';
                    $('#<%=hidIsShowAdvancedSearch.ClientID %>').val(0);
                } else {                    
                    displayMode = 'block';
                    $('#<%=hidIsShowAdvancedSearch.ClientID %>').val(1);                    
                    $('#<%=btnSelect.ClientID %>').click();
                }
                
                $('#advanced_condition').css({'display': displayMode});
            });                        
        }               
        
        // --------------------------------
        
        // # �Ƿ��۵���ѯ�������������
        var isShowPanel = $('#<%=chkHiddenConditionPanel.ClientID %>').attr('checked');
                       
        isShowPanel = isShowPanel || isShowPanel == 'checked';                        
        var _display = 'none';
        if (isShowPanel) {         
            _display = 'table-row';   
            if ($.browser.msie) {    /* ����ie6 */
                if ($.browser.version == '6.0') {
                    _display = 'block';        
                }
            }                                  
        }
        
        $('#<%=trConditionList.ClientID %>').css({'display': _display});
        $('#<%=trConditionButton.ClientID %>').css({'display': _display});    
        
        // ---------------------------------
        
        // # �Ƿ���ʾ�߼��������
        var isShowAdavencedSearchPanel = $('#<%=hidIsShowAdvancedSearch.ClientID %>').val();          
        isShowAdavencedSearchPanel = parseInt(isShowAdavencedSearchPanel);      
        
        if (isShowAdavencedSearchPanel == 0) {
            $('#advanced_condition').hide();
        } else {
            $('#advanced_condition').show();
        }
        
        // ---------------------------------
                
        // # ɾ��ѡ��Ĳ�ѯ���
        $('#delete_query_item').click(function(){
            if (confirm("ȷ��ɾ����?")) {
                $('#<%=btnDelete.ClientID %>').click();
            }
        });
        
        // ---------------------------------
        
        // # ��ѡ��[��ѯȫ��], ����ʾɾ����ť.        
        var _key = $('#<%=ddlCondition.ClientID %> option:selected').attr('value');    
        _key = parseInt(_key);
        
        if (_key > -1) {
            $('#delete_query_item').show();
        }       
       
    }catch(e)
    {
        alert(e.message);
    }
});

$('#advanced_condition_collapsed_left_keywords').click(function(){
    try{    
        var displayMode = $('#<%=trConditionList.ClientID %>').css('display');             
           
        if (displayMode != 'none') {
            displayMode = 'none';
            $('#<%=chkHiddenConditionPanel.ClientID %>').removeAttr('checked');
        } else {
            displayMode = 'table-row';
            if ($.browser.msie) {    /* ����ie6 */
                if ($.browser.version == '6.0') {
                    displayMode = 'block';        
                }
            }              
            $('#<%=chkHiddenConditionPanel.ClientID %>').attr('checked','checked');
        }
         
        $('#<%=trConditionList.ClientID %>').css({'display': displayMode});
        $('#<%=trConditionButton.ClientID %>').css({'display': displayMode});    
    }catch (e) { 
        alert(e.message);
    }
});
</script>

