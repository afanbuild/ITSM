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
        //打印
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
    <input id="btnPrint" type="button" value="打  印" style="width:60px" onclick="printdiv();"/>
    <input id="btnClose" type="button" value="关  闭" style="width:60px" onclick="window.close();"/>
      <div id="PrintArea">
                <div id="head">事 件 单</div>
			<div id="head_line">事件单号：<asp:Label ID="lblID" runat="server" Text="&nbsp;"></asp:Label>&nbsp;&nbsp;登记时间：<asp:Label ID="lblTime" runat="server" Text="&nbsp;"></asp:Label>&nbsp;&nbsp; 
				受理人：<asp:Label ID="lblPerson" runat="server" Text="&nbsp;"></asp:Label></div>
			<div id="main">
				<div id="line1">
					<div class="unit_normal" style="WIDTH: 80px; LINE-HEIGHT: 50px">客户名称</div>
					<DIV class="unit_normal" style="WIDTH: 140px; HEIGHT: 50px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:Label ID="lblCustomName" runat="server" Text="&nbsp;"></asp:Label></td>
							</tr>
						</table>
					</DIV>
					<div class="unit_normal" style="WIDTH: 60px; LINE-HEIGHT: 50px">联系人</div>
					<DIV class="unit_right" style="WIDTH: 150px; HEIGHT: 50px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:Label ID="lblLxr" runat="server" Text="&nbsp;"></asp:Label></td>
							</tr>
						</table>
					</DIV>
				</div>
				<div id="line2" style="BORDER-BOTTOM: 3px double">
					<div style="WIDTH: 120px; LINE-HEIGHT: 90px; TEXT-ALIGN: center">详细描述：</div>
					<div style="MARGIN-LEFT: 5px; WIDTH: 301px; MARGIN-RIGHT: 5px; HEIGHT: 90px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:Label ID="lblContent" runat="server" Text="&nbsp;"></asp:Label></td>
							</tr>
						</table>
					</div>
				</div>
				<div id="line2">
					<div style="WIDTH: 120px; LINE-HEIGHT: 90px; TEXT-ALIGN: center">措施及结果：</div>
					<div style="MARGIN-LEFT: 5px; WIDTH: 300px; MARGIN-RIGHT: 5px; HEIGHT: 90px">
						<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>&nbsp;</td>
							</tr>
						</table>
					</div>
				</div>
				<div id="line2">
					<div style="MARGIN-LEFT: 5px; WIDTH: 320px; LINE-HEIGHT: 25px; HEIGHT: 100%; TEXT-ALIGN: left">满意&nbsp;□&nbsp; 
						&nbsp;&nbsp;基本满意&nbsp;□&nbsp;&nbsp;&nbsp;不满意&nbsp;□&nbsp;&nbsp;&nbsp; 客户验收签名：</div>
				</div>
				<div id="line3">
					<div class="unit_normal" style="WIDTH: 100px">材料名称</div>
					<div class="unit_normal" style="WIDTH: 80px">型号</div>
					<div class="unit_normal" style="WIDTH: 30px">数量</div>
					<div class="unit_normal" style="WIDTH: 40px">单价</div>
					<div class="unit_normal" style="WIDTH: 60px">材料费</div>
					<div class="unit_normal" style="WIDTH: 50px">人工费</div>
					<div class="unit_right" style="WIDTH: 70px">备注</div>
				</div>
				
				<div id="line3"><FONT face="宋体"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="宋体"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="宋体"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="宋体"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="宋体"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 28px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 28px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 28px"></div>
				</div>
				
				<div id="line3"><FONT face="宋体"></FONT>
					<div class="unit_normal" style="WIDTH: 100px; HEIGHT: 20px">费用合计</div>
					<div class="unit_normal" style="WIDTH: 80px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 30px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 40px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 60px; HEIGHT: 20px"></div>
					<div class="unit_normal" style="WIDTH: 50px; HEIGHT: 20px"></div>
					<div class="unit_right" style="WIDTH: 70px; HEIGHT: 20px"></div>
				</div>
				<div id="line2">
					<div class="unit_normal" style="MARGIN-LEFT: 5px; WIDTH: 210px; LINE-HEIGHT: 30px">合计金额：
					</div>
					<div style="MARGIN-LEFT: 4px; WIDTH: 210px; LINE-HEIGHT: 30px">上门时间：</div>
				</div>
				<div id="line2" style="BORDER-BOTTOM: 3px double">
					<div class="unit_normal" style="MARGIN-LEFT: 5px; WIDTH: 210px; LINE-HEIGHT: 30px">合计工时：
					</div>
					<div style="MARGIN-LEFT: 4px; WIDTH: 210px; LINE-HEIGHT: 30px">完成时间：</div>
				</div>
				<div id="line2">
					<div style="MARGIN-LEFT: 5px; WIDTH: 210px; LINE-HEIGHT: 30px; HEIGHT: 100%; TEXT-ALIGN: left">派单人：</div>
					<div style="MARGIN-LEFT: 4px; WIDTH: 210px; LINE-HEIGHT: 30px; HEIGHT: 100%; TEXT-ALIGN: left">服务经手人：</div>
				</div>
				<div id="line4">
					<div class="unit_normal" style="WIDTH: 60px; LINE-HEIGHT: 40px">回访人</div>
					<div class="unit_normal" style="WIDTH: 90px; HEIGHT: 40px"></div>
					<div class="unit_normal" style="WIDTH: 50px; LINE-HEIGHT: 20px; HEIGHT: 40px">回&nbsp;访<br>
						方&nbsp;式</div>
					<div class="unit_normal" style="WIDTH: 80px; LINE-HEIGHT: 20px; HEIGHT: 40px">电话&nbsp;&nbsp;□<br>
						上门&nbsp;&nbsp;□</div>
					<div class="unit_normal" style="WIDTH: 50px; LINE-HEIGHT: 20px; HEIGHT: 40px">回&nbsp;访<br>
						时&nbsp;间</div>
					<div style="WIDTH: 100px; HEIGHT: 40px"></div>
				</div>
				<div id="line4">
					<div class="unit_normal" style="WIDTH: 60px; LINE-HEIGHT: 40px">被回访人</div>
					<div class="unit_normal" style="WIDTH: 90px; HEIGHT: 40px"></div>
					<div class="unit_normal" style="WIDTH: 50px; LINE-HEIGHT: 20px; HEIGHT: 40px">回&nbsp;访<br>
						情&nbsp;况</div>
					<div style="WIDTH: 225px; LINE-HEIGHT: 40px">满意&nbsp;□&nbsp;&nbsp;&nbsp;基本满意&nbsp;□&nbsp; 
						不满意&nbsp;□
					</div>
				</div>
			</div>
          </div>
    </form>
</body>
</html>