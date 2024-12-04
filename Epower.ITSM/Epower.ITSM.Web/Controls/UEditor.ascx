<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UEditor.ascx.cs" Inherits="Epower.ITSM.Web.Controls.UEditor" %>
<script type="text/javascript" language="javascript">
    window.UEditorURL = '<%=UEditorURL %>';
    window.UEditorServer = '<%=UEditorServer %>';
    window.UEditorPort = '<%=UEditorPort %>';         
    window.UEditorFrameWidth = '<%=UEditorFrameWidth %>'; 
    window.UEditorFrameHeight = '<%=UEditorFrameHeight %>'; 
    window.UEditorReadOnly = <%=UEditorReadOnly.ToString().ToLower() %>;            
</script>

    <%--<script type="text/javascript" src="../UEditor/editor_config.js"></script>
    <script type="text/javascript" src="../UEditor/editor_all_min.js"></script>--%>
    <script type="text/javascript" src="../ueditor2/ueditor.config.js"></script>
    <script type="text/javascript" src="../ueditor2/ueditor.all.min.js"></script>
    
    <div id="myEditorDiv" runat="server">        
        <textarea runat="server" id="myEditor"></textarea>        
    </div>    
    
<script type="text/javascript">
    window.UEditorName = '<%=myEditor.ClientID %>';
    
    $(document).ready(function(){ UE.getEditor(window.UEditorName);  });
    
    
    // UE.getEditor('editor').getContentTxt()
</script>