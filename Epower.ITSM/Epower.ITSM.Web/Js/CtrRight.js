
//不可读,就一定不可做所有操作
function CanRead_Click(ctl)
{
	reg=/chkCanRead/g;
	var idtag=ctl.id.replace(reg,"");
	if (ctl.checked==false) {
		document.all(idtag+"chkCanAdd").checked=false;
		document.all(idtag+"chkCanModify").checked=false;
		document.all(idtag+"chkCanDelete").checked=false;
	}
}

