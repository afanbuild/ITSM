
//���ɶ�,��һ�����������в���
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

