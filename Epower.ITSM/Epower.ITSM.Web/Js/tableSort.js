<script language="javascript">
var lastSortCol =-1;  //最后一次排序列
var sortFlag=0;       //排序标志 0:从小到大 1:从大到小
var sortIcon=new Array(2);

sortIcon[1]="↓";   
sortIcon[0]="↑";


function sortDate(a,b)
{
        a = $(a).find('td:eq('+ window._col +')').text();
        b = $(b).find('td:eq('+ window._col +')').text();                                              
        aA  = a.split("-");
        bA = b.split("-");
        da = new Date(parseInt(aA[0]),parseInt(aA[1])-1,parseInt(aA[2]));
        db = new Date(parseInt(bA[0]),parseInt(bA[1])-1,parseInt(bA[2]));
        return db.getTime() - da.getTime();

}   

function sortTable1(idTable, col, type) {
   var container = $('#' + idTable);
   var tr_array = container.find('tr:gt(0)');    
   
   var node;
   tr_array.each(function(){
       node = $(this).find('td:eq('+col+')');
       if (node.text().trim() != '') {
           return false;
       }
   });
   
   //var node = $(tr_array[0]).find('td:eq('+ col +')');   
   var val = node.text();
   
   tr_array.sort(function(a,b){
        a = $(a).find('td:eq('+ col +')').text().trim();
        b = $(b).find('td:eq('+ col +')').text().trim();                                              
       return compare(a,b,type)
   });
   
    for(var i =0; i< tr_array.size();i++){               
        $(tr_array[i]).insertAfter(container.find('tr:eq(0)'));
    }
    
    if (sortFlag == 0) {sortFlag = 1;}
    else {sortFlag = 0;}     
    
    return;
    
    function NumberColumnSortFunc(a, b) {        
        a = $(a).find('td:eq('+ col +')').text().trim();
        b = $(b).find('td:eq('+ col +')').text().trim();                                              
        
        try{                                               
            a = parseInt(a);
            b = parseInt(b);
        }catch(e) {
            alert(e.message);
        }               
        
        if (sortFlag == 0) {
            return a - b;
        } else {
            return b - a;
        }    
    }
    
    function DateColumnSortFunc(a, b){        
        var c = a;
        var d = b;
        a = $(a).find('td:eq('+ col +')').text().trim();
        b = $(b).find('td:eq('+ col +')').text().trim();                                              
        
        try{                                   
            if (a == '') {
                a = '1900-1-1 11:11:11';                                
            }else if (b == '') {
                b = '1900-1-1 11:11:11';                                
            }
            
            a = Date.parse(a);
            b = Date.parse(b);                                                                   
        }catch(e) {
            alert(e.message);
        }               
        
        if (sortFlag == 0) {
            return a - b;
        } else {
            return b - a;
        }    
    }
    
    function CharColumnSortFunc(a, b){
        a = $(a).find('td:eq('+ col +')');   
        b = $(b).find('td:eq('+ col +')');   
        
//        if (a.find('span').size() > 0) {
//            a = a.find('span').text();
//            b = a.find('span').text();
//        } else {
//            a = a.text();
//            b = b.text();
//        }
        a = a.text().trim(); b = b.text().trim();               
               alert(a); alert(b);
//        if(a > b)
//		{
//		    result=1;
//		}
//	    else if(a < b)
//		{
//			result=-1;
//		}
//		else if(a ==b)
//		{
//			result= 0;
//		}
//		        
        if (sortFlag == 0) {                                                
            return a.localeCompare(b);
        } else {                                    
            return b.localeCompare(a);            
        }   
    }
}

//对表进行排序.
function sortTable(idTable,col,type)
{
	//alert('0');
    var obj=document.getElementById(idTable);
	if(!obj)
	{
		alert("表未定义,排序失败!");
	}

    //设置排序标志位
    if(lastSortCol==col)
	{
		sortFlag= (sortFlag ==0 ? 1:0);
	}
    else
	{
		sortFlag=0;
	}
	//alert('1');
    //用选择排序对表内容进行排序
    var data=obj.rows;
    if(sortFlag==0) //升序排序
    {
		//alert('1');
		for(var i=1;i<data.length-1;i++)
		{
			var t = i;
			for(var j=i+1;j<data.length;j++)
			{
				//alert("i="+i+",t="+t+",j="+j+",col="+col);
				if(compare(data[t].cells[col].innerHTML+"", data[j].cells[col].innerHTML+"", type)>0)
				{
					 t=j;
				}
			}

			if(t!=i)
			{
				swap(obj,i,t);
			}
		}
    }
	else           //降序排序
	{
		for(var i=1;i<data.length-1;i++)
		{
			var t = i;
			
			for(var j=i+1;j<data.length;j++)
			{
				//alert("i="+i+",t="+t+",j="+j+",col="+col);
				if(compare(data[t].cells[col].innerHTML+"", data[j].cells[col].innerHTML+"", type)<0)
				{
					 t=j;
				}
			}

			if(t!=i)
			{
				swap(obj,i,t);
			}
		}
	}

    //alert('3');
	changeTableHead(obj,col);
    lastSortCol = col;
}

//排序时改变表头内容,加排序标志.
function changeTableHead(obj,curSortCol)
{
	if(lastSortCol>=0)
	{
	    cutSortFlag(obj.rows[0].cells[lastSortCol]);
	}

	if(curSortCol>=0)
	{
        addSortFlag(obj.rows[0].cells[curSortCol]);
	}

    //去掉上次排序列排序标志.
	function cutSortFlag(obj)
	{
        var str="" + obj.innerHTML;
		str=str.replace(sortIcon[0],"");
        str=str.replace(sortIcon[1],"");
		obj.innerHTML=str;
	}

    //给排序列加标志.
	function addSortFlag(obj)
	{
		var icon=sortIcon[sortFlag];
		var str="" + obj.innerHTML;
        var pos= str.length ;

		//查找插入位置.
		
        //在td末尾加入排序标志.
	    if(pos>0)
		{
            var strEnd=str.substring(pos,str.length)
            str = str.substring(0,pos) + icon + strEnd;
		}

        obj.innerHTML=str;
	}
}

//交换表第i与第j行.
function swap(obj,i,j)
{
//    try {
//        $(obj).find('tr:eq('+i+')')
//        .appendTo($(obj).find('tr:eq('+j+')'));
//    $(obj).find('tr:eq('+ j - 1 +')')
//        .appendTo($(obj).find('tr:eq('+i+')'));        
//    }catch(e) {
//        alert(e.message);
//    }
    if (obj.moveRow) {
        obj.moveRow(i,j);    
        obj.moveRow(j-1,i);
    } else {       
        moveRow(obj, i, j);
        moveRow(obj, j - 1, i);
    }
}

function moveRow(table, from, to)
{
  var tbody = table.tBodies[0]; // Use tbody
  var trFrom = tbody.rows[from]; // Make sure row stays referenced
  tbody.removeChild(trFrom); // Remove the row before inserting it (dupliate id's etc.)
  var trTo = tbody.rows[to];
  tbody.insertBefore(trFrom, trTo);
}

//比较a,b大小
function compare(a,b,type)
{
	var result;

    //特殊字符
	if(isSpecial(a)&&isSpecial(b))
	{
		return getSpecialType(a) - getSpecialType(b);
	}	
	if(isSpecial(a))
	{
		return 1;
	}
	if(isSpecial(b))
	{
		return -1;
	}

    if(type==1)     //数字类型
	{
	    return parseFloat(a) - parseFloat(b);
	}else if ( type == 2 ) {    // 日期类型
	    return Date.parse(a) - Date.parse(b);
	}
	else       //字符类型
	{
        if(a > b)
		{
		    result=1;
		}
	    else if(a < b)
		{
			result=-1;
		}
		else if(a ==b)
		{
			result= 0;
		}
	}
	
	return result;

	//判断是否正常字符串
	function isSpecial(a)
	{
		if(a==""||a=="--")
		   return true;
		else
		   return false;
	}

	//获取特殊字符类型号
	function getSpecialType(a)
	{
		if(a=="")
		   return 0;
		else if(a=="--")
		   return -1;
		else 
		   return -99;
	}
}

</script>