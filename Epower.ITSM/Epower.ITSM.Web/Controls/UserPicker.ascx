<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserPicker.ascx.cs" Inherits="Epower.ITSM.Web.Controls.UserPicker" %>

<script>
    String.prototype.trim = function()  //去空格
    {
        // 用正则表达式将前后空格
        // 用空字符串替代。
        return this.replace(/(^\s*)|(\s*$)/g, "");
    }

    function SelectReceiver(obj) {

        var DeptID = document.getElementById("<%=HidDept.ClientID%>").value;
        var url = '<%=sApplicationUrl %>' + "/mydestop/frmSelectPerson.aspx?DeptID=" + DeptID + "&randomid=" + GetRandom();
        url = url + "&Opener_ClientId="+obj.id;   
        open(url, "E8WinSon", "resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=400,height=300px,left=150,top=100");
    }
    function QSelectPerson(obj)   //快速发送
    {
        var newDateObj = new Date()
        var sparamvalue = newDateObj.getYear().toString() + newDateObj.getMonth().toString();
        var name = obj.value.trim();

        var value = window.showModalDialog('<%=sApplicationUrl %>' + "/mydestop/frmQuickLocateUser.aspx?UserName=" + escape(name), "", "dialogHeight:500px;dialogWidth:400px");
        if (value != null) {
            if (value.length > 1) {
                document.getElementById(obj.id.replace("txtUser", "hidUser")).value = value[0];
                obj.value = value[1];
                document.getElementById(obj.id.replace("txtUser", "hidUserName")).value = obj.value;
            }
        }
        if (typeof (UserPickerChange) != "undefined") {
            UserPickerChange();
        }
    }

    function EnterKey() { if (event.keyCode == 13) GetUserID(); }

    var xmlhttp = null;
    function CreateXmlHttpObject() {
        try {
            xmlhttp = new ActiveXObject("MSXML2.XMLHTTP");
        }
        catch (e) {
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e2) { }
        }
        return xmlhttp;
    }

    function txtonchang() {
        if (document.getElementById('<%=txtUser.ClientID%>').fireEvent) {
            document.getElementById('<%=txtUser.ClientID%>').fireEvent('onchange');
        }
        else {
            document.getElementById('<%=txtUser.ClientID%>').onchange();
        }
    }

    function GetUserID(obj) {

        if (obj.value.trim() == "") {
            document.getElementById(obj.id.replace("txtUser", "hidUser")).value = "0";  //
            document.getElementById(obj.id.replace("txtUser", "hidUserName")).value = "";
            return;
        }
        if (obj.value.trim() == document.getElementById(obj.id.replace("txtUser", "hidUserName")).value.trim() && (document.getElementById(obj.id.replace("txtUser", "hidUser")).value.trim() != "" || document.getElementById(obj.id.replace("txtUser", "hidUser")).value.trim() != "0")) {
            return;
        }
        //==================zxl==
           $.ajax({
                url:'<%=sApplicationUrl %>' + "/MyDestop/frmQuickUserXmlHttp.aspx?UserName=" + escape(obj.value),
                datatype:"json",
                type:'GET',
                success:function(data){
                   
                    if(data =="-1")
                    {
                       alert("此用户不存在！");
                       obj.value="";
                       obj.focus(); 
                    }
                    else if(data=="0")
                    {
                    QSelectPerson(obj);
                    
                    }
                    else{ //找到唯一的
                   //  debugger;
                     //这只是是转换json格式:如:name:'6102',value:'天津市'
                      //  var json =eval("("+data.d+")");
                     
                      var json=data;
                        
                         var i = json.indexOf(",");
                         document.getElementById(obj.id.replace("txtUser", "hidUser")).value = json.substring(0, i);
                            obj.value = json.replace(document.getElementById(obj.id.replace("txtUser", "hidUser")).value + ",", "");
                            document.getElementById(obj.id.replace("txtUser", "hidUserName")).value = obj.value;
                            if (typeof (UserPickerChange) != "undefined") {
                                UserPickerChange();
                            }
                    }
                },
                error:function()
                {
                  //  alert("错误！");
                }
            
           });
        
    }
</script>
<input id="hidClientId_ForOpenerPage" runat="server" name="hidClientId_ForOpenerPage" type="hidden" />
<asp:Label ID="labUser" runat="server" Visible="False"></asp:Label>
<asp:TextBox ID="txtUser"   runat="server" MaxLength="80" onblur="GetUserID(this);"></asp:TextBox>&nbsp;
<input
        id="cmdPopUser" runat="server" name="cmdPopUser" onclick="SelectReceiver(this)"
        type="button" value="..." class="btnClass2" />
 <input id="hidUser" runat="server"
            name="hidUser" size="4" style="width: 56px; height: 19px" type="hidden" />
<input id="hidUserName" runat="server" name="hidUser" size="4" style="width: 56px;
    height: 19px" type="hidden" />
<asp:Label ID="rWarning" runat="server" Font-Bold="False" Font-Size="Small" ForeColor="Red">*</asp:Label>
<input id="HidDept" runat="server" type="hidden" value="1" />