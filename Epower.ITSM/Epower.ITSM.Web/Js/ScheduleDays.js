function renderDay(owner, day, evt)
{
	if(ScheduleDays == null) return;
	var d, back = "", url = "";
	var i = ScheduleDays.length;
	while(i-- > 0)
	{
		d = ScheduleDays[i];
		if(d == null || d.length < 5) continue;
		if((d[0] == -1 || d[0] == day.year) && (d[1] < 0 || d[1] == day.month) && d[2] == day.day)
		{
			if(!day.selected) back = d[3];
			url = d[4];
			if(url.length > 0) url = "url(\"" + url + "\")";
			break;
		}
	}
	day.element.style.backgroundColor = back;
	day.element.style.backgroundImage = url;
	day.element.style.backgroundRepeat = "no-repeat";
}
function dayChange(owner, date, evt)
{
	if(ScheduleDays == null || date == null) return;
	var year = date.getFullYear(), month = date.getMonth() + 1, day = date.getDate();
	var back = "", url = "";
	var bell = false;
	var elem = ig_csom.getElementById("NoBell");
	if(elem != null) bell = elem.checked;
	var i = ScheduleDays.length;
	while(i-- > 0)
	{
		var d = ScheduleDays[i];
		if(d == null || d.length < 5) continue;
		if((d[0] == -1 || d[0] == year) && (d[1] < 0 || d[1] == month) && d[2] == day)
		{
			back = d[3];
			url = d[4];
			if(bell && d[4] == "./images/bell.gif")
				evt.cancel = true;
		}
	}
	/*
	elem = ig_csom.getElementById("ShowSelected");
	if(elem == null){alert("no ShowSelected"); return;}
	if(back.length > 0 || url.length > 0)
		back = "Color:&nbsp;\"" + back + "\"<br>Image:&nbsp;\"" + url + "\"";
	if(evt.cancel) back = "<b>CANCEL SELECTION</b><br><br>" + back;
	elem.innerHTML = back;
	*/
}

