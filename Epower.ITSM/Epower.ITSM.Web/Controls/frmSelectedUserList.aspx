<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmSelectedUserList.aspx.cs" Inherits="Epower.ITSM.Web.Controls.frmSelectedUserList"
    Title="已选中人员列表" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        div.wrapper
        {
            width: 100%;
            margin-left: 15px;           
        }
        .user_list
        {
            float: left;
            font-size: 14px;
        }
        div.button_list
        {
            float: left;
            margin-left: 20px;
            margin-top: 20px;
            clear: both;
            width:180px;
        }
        div.user
        {
            float: left;
            margin-top: 10px;
            margin-left: 10px;
            border: 1px solid gray;
        }
        div.body
        {
            float: left;
            width: 150px;
            height: 32px;
        }
        div.opr
        {
            float: left;
            width: 50px;
            height: 42px;
            padding-top: 10px;
            background-color: #D8D8D8;
            border-left: 1px solid gray;
            border-bottom: 1px solid gray;
            
            cursor: pointer;
        }
        div.person
        {
            float: left;
            margin-left: 10px;
            margin-top: 10px;
        }
        
     
        
        div.name
        {
            float: left;
            margin-left: 10px;
            margin-top: 10px;
            overflow: hidden;
            width: 100px;
            height: 32px;
        }
        
        
    </style>
    <div class="wrapper">
        <div class="user_list">
            <div class="user" style="display: none;">
                <div class="body">
                    <div class="person">
                        <img src="../Images/PERSON.GIF" /></div>
                    <div class="name" username="" userid="">
                    </div>
                </div>
                <div class="opr" onclick="remove_user(this)" 
                    onmouseover="this.style.backgroundColor='#04B4AE';"
                    onmouseout="this.style.backgroundColor='#D8D8D8'">
                    移除
                </div>
            </div>
        </div>
        <div class="button_list">
            <input type="button" value="确定" class="btnClass" onclick="confirm();" />
            <input type="button" value="取消" class="btnClass" onclick="window.close();" />
        </div>
    </div>


    <script type="text/javascript">
    $('#ctrtabbuttons').hide();
    
    $(document).ready(function(){
        var txtUser = window.opener.document.getElementById("<%=Opener_ClientId %>txtUser");        
        var hidUser = window.opener.document.getElementById("<%=Opener_ClientId %>hidUser");        
        var hidUserName = window.opener.document.getElementById("<%=Opener_ClientId %>hidUserName");        
        
        var arrUserID = hidUser.value.split(',');
        var arrUserName = hidUserName.value.split(',');        
        
        if (arrUserID.length <=1 && arrUserID[0] == '') {
            return;
        }
        
        for (var index = 0; index < arrUserID.length; index++ ) {
            var userid = arrUserID[index];
            var username = arrUserName[index];
            
            if (username == '') { continue; };
            
            var userTpl = $('.user_list .user:first').clone(true);
            
            var name = userTpl.find('.name');
            name.text(username);
            name.attr('username', username);
            name.attr('userid', userid);
            
            
            $('.user_list').append(userTpl);
            userTpl.show();
        }
        
    });
    
    function confirm() {
        var txtUser = window.opener.document.getElementById("<%=Opener_ClientId %>txtUser");        
        var hidUser = window.opener.document.getElementById("<%=Opener_ClientId %>hidUser");        
        var hidUserName = window.opener.document.getElementById("<%=Opener_ClientId %>hidUserName");                
        
        var arrUserID = [], arrUserName = [];
        $('.user_list .user').each(function(i) {
            if (i == 0) {
                return;
            }
            
            var name = $(this).find('.name');
            arrUserName.push(name.attr('username'));
            arrUserID.push(name.attr('userid'));                        
        });        
        
        txtUser.value = arrUserName.join(',');
        hidUserName.value = arrUserName.join(',');
        hidUser.value = arrUserID.join(',');
        
        window.close();
    }
    
    function remove_user(obj) {
        $(obj).parent().hide(200).remove();               
    }
    </script>

</asp:Content>
