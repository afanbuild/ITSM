<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Epower.ITSM.Web.Common.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
     <div>
       <center> <h2>C#操作VFP的dbf数据库文件实例</h2><hr />
        <asp:gridview Width="400px" ID="Gridview1" runat="server"></asp:gridview>
            <asp:Button ID="btnInti" runat="server" OnClick="btnInti_Click" Text="createTable" /> 
        <br />
        <asp:Button ID="btnInsert" runat="server" OnClick="btnInsertOle_Click" Text="btnInsert" />
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdateOle_Click" Text="btnUpdate" />
        <asp:Button ID="btnDelete" runat="server" OnClick="btnDeleteOle_Click" Text="btnDelete" />       
        <br />
        <asp:Button ID="btnShowWithOledb" runat="server" OnClick="btnShowWithOledb_Click"
            Text="showWithOledb" />
        <asp:Button ID="btnShowWithOdbc" runat="server" OnClick="btnShowWithOdbc_Click" Text="showWithOdbc" Visible="false" />
        </center>
    </div> 
      <span style="color: red">* 首先:在e盘下创建testDB的VFP数据库.然后可使用vs2005测试.
      其中必须要先创建表，即：点击createTable按钮。</span>
        <br />
        姓：<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        TEST：<asp:TextBox ID="txtTest" runat="server"></asp:TextBox>
        <asp:Button ID="btnUp" runat="server" Text="上传表单" OnClick="btnUp_Click" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="上传文档" />
    </form>
</body>
</html>
