//duanqs
//位于屏幕中心，加载提示
function OpenNoBarWindow(url,width,height)
{
	var wWidth = width;
	var wHeight = height;
	var wLeft = (window.screen.width - wWidth) / 2;
	var wTop = (window.screen.height - wHeight-30) / 2;
	if( wLeft<200)
		wLeft=0;
	if(wTop<200)
		wTop=0;
	var winTemp = window.open("","ManageVer","top="+wTop+",left="+wLeft+",height="+wHeight+",width="+wWidth+",status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes");
	winTemp.focus();
	//winTemp.document.write("<html><head></head><body bgcolor=\"#f5f5f5\"><table width=100% height=100%><tr><td align=center valign=middle style=\"color:red;font-size:18px;\">Please wait...</td></tr></table></body></html>");
	winTemp.location.href = url;
}
function OpenNoBarWindowNew1(url,width,height)
{
	var wWidth = width;
	var wHeight = height;
	var wLeft = (window.screen.width - wWidth) / 2;
	var wTop = (window.screen.height - wHeight-30) / 2;
	var winTemp = window.open("","ManageVer","top="+wTop+",left="+wLeft+",height="+wHeight+",width="+wWidth+",status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no");
	winTemp.focus();
	winTemp.location.href = url;
}

function OpenNoBarWindow2(url,width,height)
{
   // alert("sdfds");
	var wWidth = width;
	var wHeight = height;
	var wLeft = (window.screen.width - wWidth) / 2;
	var wTop = (window.screen.height - wHeight-30) / 2;
	if( wLeft<200)
		wLeft=0;
	if(wTop<200)
		wTop=0;
var winTemp = window.open(url, "ManageVer", "top=" + wTop + ",left=" + wLeft + ",height=" + wHeight + ",width=" + wWidth + ",status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no");
	winTemp.focus();
	//winTemp.document.write("<html><head></head><body bgcolor=\"#f5f5f5\"><table width=100% height=100%><tr><td align=center valign=middle style=\"color:red;font-size:18px;\">Please wait...</td></tr></table></body></html>");
	//winTemp.location.href = url;
}