<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PZh_Issue.aspx.cs" Inherits="Epower.ITSM.Web.AppForms.PrintReport.PZh_Issue" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
		<style type="text/css"> 
		<!-- #head { height: 30px; width: 440px; font-size: 20px; line-height: 30px; word-break:break-all; margin-right: auto; margin-left: auto; text-align: center; font-weight: bold; }
	#head_line { height: 24px; width: 440px; font-size: 12px; line-height: 24px; word-break:break-all; margin-right: auto; margin-left: auto; text-align: center; font-weight: normal; vertical-align: bottom; margin-top: 2px; }
	#main{ width: 440px; margin-right: auto; margin-left: auto; border-top-width: 1px; border-left-width: 1px; border-top-style: solid; border-left-style: solid; border-top-color: #000000; border-left-color: #000000; padding: 0px; margin-top: 2px; border-right-width: 1px; border-bottom-width: 1px; border-right-style: solid; border-right-color: #000000; border-bottom-color: #000000; }
	#line1{ height: 35px; position: relative; width: 100%; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #000000; }
	#line1 div{ FLOAT: left; font-size: 12px; word-break:break-all; text-align: center; }
	#line2{
	width: 100%;
	margin:0px;
	border-bottom-width: 1px;
	border-bottom-style:solid;
	border-bottom-color: #000000;
	position: relative;
}
	#line2 div{ FLOAT: left; font-size: 12px; word-break:break-all; vertical-align: middle; }
	.unit_normal{ border-right-width: 1px; border-right-style: solid; border-right-color: #000000; }
	#line3{ height: 20px; position: relative; width: 100%; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #000000; }
	#line3 div{ FLOAT: left; font-size: 12px; word-break:break-all; line-height: 20px; text-align: center; }
	#line4{ height: 40px; position: relative; width: 100%; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #000000; }
	#line4 div{ FLOAT: left; font-size: 12px; word-break:break-all; line-height: 16px; text-align: center; }
	body { margin-left: 0px; margin-top: 0px; margin-right: 0px; margin-bottom: 0px;}
	--> 
		</style>
    <script language="javascript" type="text/javascript">
        //��ӡ
        function printdiv()
        {
            var headstr = "<html><head><title></title></head><body>";
            var footstr = "</body></html>";
            var newstr = document.all.item("PrintArea").innerHTML;
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr+newstr+footstr;
            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="btnPrint" type="button" value="��  ӡ" style="width:60px" onclick="printdiv();"/>
    <input id="btnClose" type="button" value="��  ��" style="width:60px" onclick="window.close();"/>
      <div id="PrintArea">
                <div id="head">�� �� ��</div>
			<div id="head_line">�¼����ţ�<asp:Label ID="lblID" runat="server" Text="&nbsp;"></asp:Label>&nbsp;&nbsp;�Ǽ�ʱ�䣺<asp:Label ID="lblTime" runat="server" Text="&nbsp;"></asp:Label>&nbsp;&nbsp; 
				�����ˣ�<asp:Label ID="lblPerson" runat="server" Text="&nbsp;"></asp:Label></div>
			<div id="main">
				<div id="line1">
					<div class="unit_normal" style="WIDTH: 80px; LINE-HEIGHT: 50px">�ͻ�����</div>
					<DIV class="unit_normal" style="WIDTH: 140px; HEIGHT: 50px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:Label ID="lblCustomName" runat="server" Text="&nbsp;"></asp:Label></td>
							</tr>
						</table>
					</DIV>
					<div class="unit_normal" style="WIDTH: 60px; LINE-HEIGHT: 50px">��ϵ��</div>
					<DIV class="unit_right" style="WIDTH: 150px; HEIGHT: 50px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:Label ID="lblLxr" runat="server" Text="&nbsp;"></asp:Label></td>
							</tr>
						</table>
					</DIV>
				</div>
				<div id="line2" style="BORDER-BOTTOM: 3px double">
					<div style="WIDTH: 120px; LINE-HEIGHT: 90px; TEXT-ALIGN: center">��ϸ������</div>
					<div style="MARGIN-LEFT: 5px; WIDTH: 301px; MARGIN-RIGHT: 5px; HEIGHT: 90px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:Label ID="lblContent" runat="server" Text="&nbsp;"></asp:Label></td>
							</tr>
						</table>
					</div>
				</div>
				<div id="line2">
					<div style="WIDTH: 120px; LINE-HEIGHT: 90px; TEXT-ALIGN: center">��ʩ�������</div>
					<div style="MARGIN-LEFT: 5px; WIDTH: 300px; MARGIN-RIGHT: 5px; HEIGHT: 90px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>&nbsp;</td>
							</tr>
						</table>
					</div>
				</div>
				<div id="line2">
					<div style="MARGIN-LEFT: 5px; WIDTH: 320px; LINE-HEIGHT: 25px; HEIGHT: 100%; TEXT-ALIGN: left">����&nbsp;��&nbsp; 
						&nbsp;&nbsp;��������&nbsp;��&nbsp;&nbsp;&nbsp;������&nbsp;��&nbsp;&nbsp;&nbsp; �ͻ�����ǩ����</div>
				</div>
				<div id="line3">
					<div class="unit_normal" style="WIDTH: 100px">��������</div>
					<div class="unit_normal" style="WIDTH: 80px">�ͺ�</div>
					<div class="unit_normal" style="WIDTH: 30px">����</div>
					<div class="unit_normal" style="WIDTH: 40px">����</div>
					<div class="unit_normal" style="WIDTH: 60px">���Ϸ�</div>
					<div class="unit_normal" style="WIDTH: 50px">�˹���</div>
					<div class="unit_right" style="WIDTH: 70px">��ע</div>
				</div>
				
				<div id="line3"><FONT face="����"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="����"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="����"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="����"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="����"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="����"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 20px">���úϼ�</div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 20px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 20px"></div>
				</div>
				<div id="line2">
					<div class="unit_normal" style="MARGIN-LEFT: 5px; WIDTH: 210px; LINE-HEIGHT: 30px">�ϼƽ�
					</div>
					<div style="MARGIN-LEFT: 4px; WIDTH: 210px; LINE-HEIGHT: 30px">����ʱ�䣺</div>
				</div>
				<div id="line2" style="BORDER-BOTTOM: 3px double">
					<div class="unit_normal" style="MARGIN-LEFT: 5px; WIDTH: 210px; LINE-HEIGHT: 30px">�ϼƹ�ʱ��
					</div>
					<div style="MARGIN-LEFT: 4px; WIDTH: 210px; LINE-HEIGHT: 30px">���ʱ�䣺</div>
				</div>
				<div id="line2">
					<div style="MARGIN-LEFT: 5px; WIDTH: 210px; LINE-HEIGHT: 30px; HEIGHT: 100%; TEXT-ALIGN: left">�ɵ��ˣ�</div>
					<div style="MARGIN-LEFT: 4px; WIDTH: 210px; LINE-HEIGHT: 30px; HEIGHT: 100%; TEXT-ALIGN: left">�������ˣ�</div>
				</div>
				<div id="line4">
					<div class="unit_normal" style="WIDTH: 60px; LINE-HEIGHT: 40px">�ط���</div>
					<div class="unit_normal" style="WIDTH: 90px; HEIGHT: 40px"></div>
					<div class="unit_normal" style="WIDTH: 50px; LINE-HEIGHT: 20px; HEIGHT: 40px">��&nbsp;��<br>
						��&nbsp;ʽ</div>
					<div class="unit_normal" style="WIDTH: 80px; LINE-HEIGHT: 20px; HEIGHT: 40px">�绰&nbsp;&nbsp;��<br>
						����&nbsp;&nbsp;��</div>
					<div class="unit_normal" style="WIDTH: 50px; LINE-HEIGHT: 20px; HEIGHT: 40px">��&nbsp;��<br>
						ʱ&nbsp;��</div>
					<div style="WIDTH: 100px; HEIGHT: 40px"></div>
				</div>
				<div id="line4">
					<div class="unit_normal" style="WIDTH: 60px; LINE-HEIGHT: 40px">���ط���</div>
					<div class="unit_normal" style="WIDTH: 90px; HEIGHT: 40px"></div>
					<div class="unit_normal" style="WIDTH: 50px; LINE-HEIGHT: 20px; HEIGHT: 40px">��&nbsp;��<br>
						��&nbsp;��</div>
					<div style="WIDTH: 225px; LINE-HEIGHT: 40px">����&nbsp;��&nbsp;&nbsp;&nbsp;��������&nbsp;��&nbsp; 
						������&nbsp;��
					</div>
				</div>
			</div>
          </div>
    </form>
</body>
</html>