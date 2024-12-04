<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmUserDefinedPage.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.frmUserDefinedPage"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function PrintTable() {
            document.getElementById("PrintHide").style.visibility = "hidden"
            print();
            document.getElementById("PrintHide").style.visibility = "visible"
        }

        function formset(str) {
            var cValue = parseInt(20000 * Math.random());
            //Math.random()

            //常规控件
            if (str == "1") {
                eWebEditor1.insertHTML('<input id="Text1"  name="' + cValue + '"  type="text"  style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }

            if (str == "2") {
                eWebEditor1.insertHTML('<textarea id="TextArea1" name="' + cValue + '"   cols="20" rows="2"  style="SCROLLBAR-FACE-COLOR:   #AAAAAA;   SCROLLBAR-HIGHLIGHT-COLOR:   #D8D8D8;  SCROLLBAR-SHADOW-COLOR:   #D8D8D8;   SCROLLBAR-3DLIGHT-COLOR:   #D8D8D8;   SCROLLBAR-ARROW-COLOR:   #D8D8D8;   SCROLLBAR-TRACK-COLOR:   #D8D8D8;   SCROLLBAR-DARKSHADOW-COLOR:   #D8D8D8; border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000 " ></textarea>');

            }

            if (str == "3") {
                eWebEditor1.insertHTML('<input id="Checkbox1"  name="' + cValue + '"   type="checkbox"  />');
            }

            if (str == "12") {
                eWebEditor1.insertHTML('<input id="Text1"  name="' + cValue + '"  type="text"  style="IME-MODE: disabled;border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"  onkeypress="var k=event.keyCode; return (k>=48&&k<=57)||k==46" onpaste="return !/\D/.test(clipboardData.getData(\'text\'))"  ondragenter="return false"/>');
            }

            if (str == "13") {
                var num = Math.random();
                eWebEditor1.insertHTML('<input id="' + num + '" name="' + cValue + '"  type="text"  onclick="setday(this)"   value=""  style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }

            //宏控件




            if (str == "4") {
                eWebEditor1.insertHTML('<input readonly id="Text2" name="' + cValue + '"  type="text" value="宏控件-用户姓名" style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }

            if (str == "5") {
                eWebEditor1.insertHTML('<input readonly id="Text2" name="' + cValue + '"  type="text" value="宏控件-用户部门" style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }

            if (str == "6") {
                eWebEditor1.insertHTML('<input readonly id="Text2"  name="' + cValue + '"   type="text" value="宏控件-用户角色" style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }

            if (str == "7") {
                eWebEditor1.insertHTML('<input readonly id="Text2"  name="' + cValue + '"  type="text" value="宏控件-用户职位" style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }


            if (str == "8") {
                eWebEditor1.insertHTML('<input readonly id="Text2"  name="' + cValue + '"  type="text" value="宏控件-当前时间(日期)" style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }

        }
    </script>

    <script language="javascript">
        function settiaojian() {
            var ziduanname = document.getElementById("DropDownList1").value.split('[')[0];
            var ziduanleixing = document.getElementById("DropDownList1").value.split('[')[1];
            if (ziduanleixing == "常规型]") {
                eWebEditor1.insertHTML('<input id="Text3"  name="' + "TIaoJianZiDuan_" + ziduanname + '"  type="text"  style="border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"/>');
            }
            else {
                eWebEditor1.insertHTML('<input id="Text3"  name="' + "TIaoJianZiDuan_" + ziduanname + '"  type="text"  style="IME-MODE: disabled;border-left:0px;border-top:0px;border-right:0px;border-bottom:1px   solid   #000000"  onkeypress="var k=event.keyCode; return (k>=48&&k<=57)||k==46" onpaste="return !/\D/.test(clipboardData.getData(\'text\'))"  ondragenter="return false"/>');
            }
        }
  
    </script>

    <table style="width: 100%" class="listContent">
        <tr>
            <td class="listTitle" align="right" nowrap style="width: 10%;">
                选择流程:
            </td>
            <td style="width: 326px;" nowrap colspan="6" align="left" class="list">
                <asp:DropDownList ID="dpdFlow" runat="server" Width="192px" AutoPostBack="True" OnSelectedIndexChanged="dpdFlow_SelectedIndexChanged">
                </asp:DropDownList>
                流程模型编号：<asp:Label ID="lblFlowModelID" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 100%" bgcolor="#999999" border="0" cellpadding="2" cellspacing="1">
        <tr>
            <td align="right" class="listTitle" style="width: 10%;">
                表单名称：
            </td>
            <td class="list">
                <asp:TextBox ID="txtPageName" runat="server" Width="350px"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none">
            <td align="right" class="listTitle">
                条件字段：
            </td>
            <td class="list">
                <asp:DropDownList ID="DropDownList1" runat="server" Width="194px">
                </asp:DropDownList>
                <input id="Button3" style="width: 69px" type="button" value="插入字段" onclick="settiaojian()" /><asp:Button
                    ID="Button2" runat="server" Text="删除" Width="38px" />字段名：<asp:TextBox ID="TextBox4"
                        runat="server" Width="85px"></asp:TextBox>
                类型：<asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem>[常规型]</asp:ListItem>
                    <asp:ListItem>[数字型]</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text="添加字段" Width="69px" />
            </td>
        </tr>
        <tr>
            <td align="center" valign="top" class="listTitle">
                <strong>标准控件</strong>
                <hr />
                <input id="Button4" onclick="formset(1)" style="width: 100px" type="button" value="插入常规输入框" />
                <br />
                <input id="Button14" onclick="formset(12)" style="width: 100px" type="button" value="插入数字输入框" />
                <br />
                <input id="Button5" onclick="formset(2)" style="width: 100px" type="button" value="插入文本输入框" /><br />
                <input id="Button15" onclick="formset(13)" style="width: 100px" type="button" value="插入日期选择" /><br />
                <input id="Button6" onclick="formset(3)" style="width: 100px" type="button" value="插入复选框" /><br />
                <hr />
                <strong>宏控件</strong>
                <hr />
                <input id="Button7" onclick="formset(4)" style="width: 100px" type="button" value="插入用户姓名" />
                <br />
                <input id="Button8" onclick="formset(5)" style="width: 100px" type="button" value="插入用户部门" />
                <input id="Button9" onclick="formset(6)" style="width: 100px" type="button" value="插入用户角色"
                    style="display: none;" />
                <input id="Button10" onclick="formset(7)" style="width: 100px" type="button" value="插入用户职位"
                    style="display: none;" />
                <br />
                <input id="Button11" onclick="formset(8)" style="width: 100px" type="button" value="当前时间(日期)" />
            </td>
            <td style="padding-left: 5px; height: 25px;" class="list">
                <asp:TextBox ID="TxtContent" runat="server" Style="display: none"></asp:TextBox>
                <iframe frameborder="0" height="350" id="eWebEditor1" scrolling="no" src="../eWebEditor/ewebeditor.htm?id=TxtContent&style=mini"
                    width="99%"></iframe>
                <br />
                <span style="color: #ff0000">1.点击左边控件按钮将会把内容插入到编辑器中鼠标的光标所在处，如果鼠标的光标没有在编辑器中将不会插入内容。
                </span>
            </td>
        </tr>
    </table>
</asp:Content>
