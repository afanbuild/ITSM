<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrFlowRemark.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.CtrFlowRemark" %>
<%--<div style=" width:800px; word-wrap: break-word">--%>
    <asp:Label ID="labCaption" runat="server" Visible="False"  Width="95%"></asp:Label>
<%--</div>--%>
<div style="width: 95%; float: left;">
    <asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" Width="100%" Rows="4"></asp:TextBox>
</div>
<div style="float: left; margin-left: 10px; margin-top: 10px;">
    <asp:Label ID="rWarning" runat="server" Style="margin-left: 7px;" Font-Bold="False"
        Font-Size="Small" ForeColor="Red">*</asp:Label>
</div>

<!-- Begin: 自动统计用户输入的字符数, 并检测是否超出最多字符限制.  - 2013-04-25 @孙绍棕 -->
<!--
请注意:
1, 统计时, 换行时被认为是两个字符. 这是因为在 textarea 中的换行只用 \n (换行) 符号标记, 而在服务端程序代码
中换行用两个字符标记: \r\n (回车和换行).
2, 若没有设置 MaxLength 属性, 则默认设置为 500 个字符.
-->

<asp:Literal ID="literalCharCount" runat="server">
<div class="inputed_char_count_container" style="clear: both; margin-bottom: 5px; margin-top: 5px; margin-left:5px; color: Gray; float: left; width:50%;">
    最多可输入 #MaxLength# 个字符。现在已输入 <label class="inputed_char_count">0</label> 个字符。
</div>

<script type="text/javascript" language="javascript">
    $(document).ready(function(){
    
        var len = $('##ContainerID#').val().length;
        
        var matched_couple = $('##ContainerID#').val().match(/\n/ig);
        if ( matched_couple ) {
            len = len + matched_couple.length;
        }               
        
        $('##ContainerID#').parent().nextAll('.inputed_char_count_container').find('.inputed_char_count').text(len);
    });

    $('##ContainerID#').keyup(function(){
        check_char_count(this, #MaxLength#);
    }).blur(function(){
        check_char_count(this, #MaxLength#);
    });
    
    function check_char_count(container, maxlength) {        
        var val = $(container).val();
        
        var len = val.length;  
        
        var matched_couple = val.match(/\n/ig);        
        if ( !matched_couple ) {
            matched_couple = [];
        }
        
        len = len + matched_couple.length;
              
        var char_count_container = $(container).parent().nextAll('.inputed_char_count_container').find('.inputed_char_count');
        
        if ( len >= maxlength ) {            
            var max = maxlength - matched_couple.length;        
            $(container).val(val.substr(0, max));
            len = $(container).val().length + matched_couple.length;
            
            char_count_container.text(len);
            
            return;
        }
        
        char_count_container.text(len);    
    }
</script>
</asp:Literal>
<!-- End. -->