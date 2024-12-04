<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Epower.ITSM.Web.FlashReoport.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
    .pageHeader{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-WEIGHT: bold;
	FONT-SIZE: 12pt;
}
.header
{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-WEIGHT: bold;
	FONT-SIZE: 9pt;
	
}
.headerwhite
{
	COLOR: #FFFFFF;
	FONT-FAMILY: Verdana;
	FONT-WEIGHT: bold;
	FONT-SIZE: 11px;
	
}
.headingtable
{
	COLOR: #FFCC00;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 9pt;
	FONT-WEIGHT: bold;
	
	PADDING-TOP: 5px;
	PADDING-BOTTOM: 5px;
}
.text
{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
}
.textBold
{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
	FONT-WEIGHT: bold;
}
.textwhite
{
	COLOR: #FFFFFF;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	FONT-WEIGHT: bold;
	
}
.highlightBlock{
	BACKGROUND: #F1F1F1;
	COLOR: #291E40;
	BORDER: 1px solid #291E40;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
	PADDING: 10px;
}
.imageCaption
{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
}
.imageBorder
{
	BORDER: 1px solid #f1f1f1;
}
.codeBlock
{
	BACKGROUND: #FFFCF0;
	COLOR: #291E40;
	FONT-FAMILY: Courier New;
	FONT-SIZE: 12px;
	
	PADDING: 10px;
}
.codeComment
{
	COLOR: #007F00;
}

.codeInline
{
	COLOR: #291E40;
	FONT-FAMILY: Courier New;
	FONT-SIZE: 12px;
	
}
.A
{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
	FONT-WEIGHT: bold;
	
}
.A:hover
{
	COLOR: #FFA000;
}
A
{
	COLOR: #291E40;
	
}
A:hover
{
	COLOR: #FFA000;
}
.trVeryLightYellowBg
{
	BACKGROUND: #FFFCF0;
}
.lightBlueTr
{
	BACKGROUND: #E0E9ED;
	BORDER-RIGHT:1px solid #94B0BE;
	BORDER-BOTTOM:1px solid #94B0BE;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
}
.greyBorderTable
{
	BORDER: 1px solid #f1f1f1;
}
.yellowBorderTable
{
	BORDER: 1px solid #FCCC22;
}

.blueTr
{
	BACKGROUND: #94B0BE;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
}
.greyTr
{
	BACKGROUND: #F1f1f1;
}
.darkGreyTr
{
	BACKGROUND: #999999;
}
.darkYellowTr
{
	BACKGROUND: #FCCC22;
}
.lightYellowTr
{
	BACKGROUND: #FEECB4;
}
.formtable
{
	COLOR: #291E40;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 8pt;
	
	FONT-WEIGHT:none;
}
.textbox
{
	BACKGROUND: #F8F8F8;
	BORDER: 1px solid #999999;
	COLOR: #999999;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 11px;
		
}

.button
{
	BACKGROUND: #F8F8F8;
	BORDER: 1px solid #999999;
	COLOR: #999999;
	FONT-FAMILY: Verdana;
	FONT-SIZE: 11px;
		
	FONT-WEIGHT: bold;
}

.borderBottom {
border-bottom:1px solid #cccccc;
}

    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
          <table width="100%">
            <tr>
               <td colspan="2">
               <input id="xx" value="保存" type="button" onclick="xxSave()" />
                         <asp:Button ID="Button1" runat="server" Text="Button" />
               </td>
            </tr>
            <tr>
                <td colspan="2">                 
                    <div id="Div1" runat="server"></div>
                    <div id="Div2" runat="server"></div>
                </td>            
            </tr>     
        </table>
    </div>
    </form>
</body>
</html>
