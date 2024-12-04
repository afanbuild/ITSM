<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="browsepic.ascx.cs" Inherits="Epower.ITSM.Web.Controls.browsepic" %>

<!-- 1.浏览本地文件，不上传，点击保存按钮时再统一上传；-->

<script type = "text/javascript" language = "javascript" >

var picid = 1; // 图片上传域的个数

function piclistCallBack(response) { 
    var json = eval("(" + response + ")");
    var record = json.record;

    var recordLength = record.length;
    for (var i = 0; i < recordLength; i++) { 
        var picid = 1;

        $("#file" + picid).attr('disabled', 'true');
        $("#btnPicAddress" + picid).attr('disabled', 'true')
         
        // 图片显示                    
        document.getElementById("img" + picid).src = record[i].imglogo.replaceAll('/','\\');
        
        document.getElementById("urlfile" + picid).value = record[i].imglogo.replaceAll('/','\\');
        
        addOrCutData(1, "hurlfile", record[i].imglogo.replaceAll('/','\\'));
    }
}


// 浏览本地文件

function onUploadImgChange(picid) {

    var sender=document.getElementById("<%=ClientID%>");  
    //var sender = document.getElementById("file" + picid);
    if (!sender.value.match(/.jpg|.gif|.png|.bmp/i)) {
        alert('图片格式无效！');
        return false;
    }
    var objPreview = document.getElementById('img' + picid);
    var objPreviewFake = document.getElementById('preview_fake' + picid);
    var objPreviewSizeFake = document.getElementById('preview_size_fake' + picid );

    if (sender.files && sender.files[0]) {
        objPreview.style.display = 'block';
//        objPreview.style.width = 'auto';
//        objPreview.style.height = 'auto';

        // Firefox 因安全性问题已无法直接通过 input[file].value 获取完整的文件路径   
        objPreview.src = sender.files[0].getAsDataURL();
        
         var image=new Image();
                image.src=objPreview.src;
                if(image.width>0 && image.height>0){
                    if(image.width/image.height>= 70/70){
                        if(image.width>70){ 
                            objPreview.width=70;
                            objPreview.height=(image.height*70)/image.width;
                        }else{
                            objPreview.width=image.width; 
                            objPreview.height=image.height;
                        }
                        objPreview.alt=image.width+"×"+image.height;
                    }
                    else{
                        if(image.height>70){ 
                            objPreview.height=70;
                            objPreview.width=(image.width*70)/image.height; 
                        }else{
                            objPreview.width=image.width; 
                            objPreview.height=image.height;
                        }
                        objPreview.alt=image.width+"×"+image.height;
                    }
                } 
        
    } else if (objPreviewFake.filters) {
        // IE7,IE8 在设置本地图片地址为 img.src 时出现莫名其妙的后果   
        //（相同环境有时能显示，有时不显示），因此只能用滤镜来解决   

        // IE7, IE8因安全性问题已无法直接通过 input[file].value 获取完整的文件路径   
        
        sender.select();
        var imgSrc = document.selection.createRange().text;

        objPreviewFake.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = imgSrc;
        objPreviewSizeFake.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = imgSrc;
        autoSizePreview( objPreviewFake,
            objPreviewSizeFake.offsetWidth, objPreviewSizeFake.offsetHeight );
        objPreview.style.display = 'none'; 
              
    }
}

function autoSizePreview( objPre, originalWidth, originalHeight ){
    var zoomParam = clacImgZoomParam( 70, 70, originalWidth, originalHeight );
    objPre.style.width = zoomParam.width + 'px';
    objPre.style.height = zoomParam.height + 'px';
    objPre.style.marginTop = zoomParam.top + 'px';
    objPre.style.marginLeft = zoomParam.left + 'px';
}

function clacImgZoomParam( maxWidth, maxHeight, width, height ){
    var param = { width:width, height:height, top:0, left:0 };

    if( width>maxWidth || height>maxHeight ){
        rateWidth = width / maxWidth;
        rateHeight = height / maxHeight;

        if( rateWidth > rateHeight ){
            param.width = maxWidth;
            param.height = height / rateWidth;
        }else{
            param.width = width / rateHeight;
            param.height = maxHeight;
        }
    }

    param.left = (maxWidth - param.width) / 2;
    param.top = (maxHeight - param.height) / 2;

    return param;
}

// 火狐下加载图片

function onPreviewLoad(picid) {

    var imgObj = document.getElementById("img" + picid);
    
    var image=new Image();
    image.src=imgObj.src;
    if(image.width>0 && image.height>0){
        if(image.width/image.height>= 70/70){
            if(image.width>70){ 
                imgObj.width=70;
                imgObj.height=(image.height*70)/image.width;
            }else{
                imgObj.width=image.width; 
                imgObj.height=image.height;
            }
            imgObj.alt=image.width+"×"+image.height;
        }
        else{
            if(image.height>70){ 
                imgObj.height=70;
                imgObj.width=(image.width*70)/image.height; 
            }else{
                imgObj.width=image.width; 
                imgObj.height=image.height;
            }
            imgObj.alt=image.width+"×"+image.height;
        }
    }

}

// 直接填写图片地址,不用上传

function picAddress_Onclick(picid) {
    var text = $("#btnPicAddress" + picid).val();
    if(text == "填写地址") {
       $("#spanPicName" + picid).html("图片地址：<input id=\"filePicAddress" + picid + "\" name=\"filePicAddress"+ picid + "\" style=\"width: 400px;\" type=\"text\" />&nbsp;图片大小：<input id=\"filePicFilesize" + picid + "\" name=\"filePicFilesize"+ picid + "\" style=\"width: 60px;\" type=\"text\" />"); 
       $("#btnPicAddress" + picid).val("返回"); 
    }   
    else if(text == "返回") {
       $("#spanPicName" + picid).html("<input id=\"file" + picid + "\" name=\"file" + picid + "\" type=\"file\" style=\"width: 400px;\" onchange=\"onUploadImgChange(" + picid + ")\" />");   
       $("#btnPicAddress" + picid).val("填写地址"); 
    }             
}

// 提交时将直接填写的内容存在到 隐藏控件中

function saveInputPicValue() {  
    var outmsg = "";        
    return outmsg;   
}       

</script>

<div id="picRoot">
    <div id="pic1">
        <div id="preview_wrapper1">
            <div id="preview_fake1" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale'); position:relative; ">
                <img id="img1" OnInit="onPreviewLoad(1)"/>  
                <span id="imgsize1"></span>                
            </div>
        </div>
        <span id="spanPicName1">
           <input id="file1" name="file1" runat="server" type="file" style="width: 400px;" onchange="onUploadImgChange(1)" />           
           <br />
           <img id="preview_size_fake1" style="filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image'); position:absolute; visibility:hidden; " />
        </span>
        <!--<input id="btnPicAddress1" type="button" value="填写地址" onclick="return picAddress_Onclick(1);" />-->
        <!-- 隐藏域保存大图地址 -->
        <input id="urlfile1" type="hidden" />     
    </div>
</div>
<input id="hurlfile" name="hurlfile" type="hidden" />
<input id="hPicid" name="hPicid" value="1" type="hidden" />
