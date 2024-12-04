<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flow_attachment_preview.aspx.cs" Inherits="Epower.ITSM.Web.Forms.flow_attachment_preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="js/swfobject.js" type="text/javascript"></script>

    <script src="js/swfobject.js" type="text/javascript"></script>
</head>
<body>
           <div id="myContent">
      <p>Alternative content</p>
    </div>     

</body>
       <script type="text/javascript">  swfobject.embedSWF("<%=FlashName%>", "myContent", "750", "500", "9.0.0");</script>
</html>
