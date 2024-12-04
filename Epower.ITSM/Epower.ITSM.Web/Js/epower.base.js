/*初始化基础对象*/
if (!window.epower) { window.epower = { tools:{ qstr:{} }, equ:{} }}

/* open new window */
epower.tools.open = function(url, target, options) {
    var _options = { scrollbars:'1', status:'no', toolbar:'no',
                     menu:'no', resizable:'1', width:'800px', height:'600px'};                                        
                     
    if (options) {                     
        for(item in options) {
            _options[item] = options[item];
        }             
    }                
        
    var _option_text = '';        
    for(var item in _options) {                        
        _option_text = _option_text + item + '=' + _options[item];
        
        if (_option_text) {
            _option_text = _option_text + ', ';
        }
    }    
    
    // remove last character.
    _option_text = _option_text.substring(0, _option_text.length - 2);
                                     
    window.open(url, target, _option_text);
};

/* Resize Window */
epower.tools.resize = function(width, height, window) {    
    if (!window) {
        window = this.window;
    }
    
    window.resizeTo(width, height);       
}

/* Reposition Window */
epower.tools.reposition = function(position, window) {
    var _position = epower.tools.computeXY(position, window);
    
    window.moveTo(_position.x, _position.y);
}

/* compute window position. */
epower.tools.computeXY = function(position, window, width, height) {
    if (!window) {
        window = this.window;
    }

    var x = window.screenLeft, y = window.screenTop, 
            _width = window.screen.width, _height = window.screen.height,
            window_width = window.innerWidth || window.document.body.offsetWidth,
            winow_height = window.innerHeight || window.document.body.offsetHeight;
                
    if (width) { window_width = width; }            
    if (height) { winow_height = height; }
    
    switch (position) {
        case 'l':
        case 'left':
        break;
        case 'r':
        case 'right':
        break;
        case 'c':
        case 'center':        
        x = (_width - window_width) / 2;  
        y = (_height - winow_height) / 2;      
        break;
    }
    
    return {x: x, y: y};
}

/* get parameter-value from window.location.search. */
epower.tools.qstr.get = function(name) {
    var req_url = window.location.search;
    
    var _r = new RegExp(name +'=([a-z|0-9|%|!]+)&?', 'i');    
    var m_obj = req_url.match(_r);    
    
    if (m_obj && m_obj.length > 1) {
        return m_obj[1];
    }
}

/* convert a object to string. */
epower.tools.obj2str = function(obj) {
    if (!obj) {
        epower.tools.error('not a object!');
        return;
    }
    
    var _text = '{ ';
    for(var item in obj) {         
        _text = _text + item + ': ' + obj[item] + ', ';
    }
    
    _text = _text.substring(0, _text.length - 2) + '}';
    
    return _text;
}

/* Begin: javascript debug tools */
epower.tools.log = function(message) {    
    console.log('[Debug] ' + (new Date()).toLocaleTimeString() + ': ' + message);
}

epower.tools.debug = function(expr, message) {
    console.assert(expr, message);
}

epower.tools.error = function(message) {
    console.error(message);
}

epower.tools.trace = function() {
    console.trace();
}

epower.tools.output = function(obj) {
    if (!obj) {
        epower.tools.error('not a object!');
        return;
    }
       
    epower.tools.log(_text);
}

/* End: javascript debug tool */

/* 清除字符串两边空格 */
String.prototype.trim = function() {	
	return this.replace(/(^\s*)|(\s*$)/g, "");
}