var moduleJs = {};
var index, jsArray, moduleName, callbackFunc;

function loadAppModuleJS(module, jsObj, callback){
	//if(jsArray instanceof Array && index < jsArray.length){
	//	throw new error("模块尚未加载完成！");
	//}

	if(moduleJs[module] != undefined){
		for (var i = 0; i < moduleJs[module].length; i++) {
			var itemId = moduleJs[module][i];
			var oldjs = document.getElementById(itemId);
			if (oldjs) oldjs.parentNode.removeChild(oldjs);
		}
	}

    if (jsObj instanceof Array) {
		index = 0;
		jsArray = jsObj;
		moduleName = module;
		callbackFunc = callback;
		moduleJs[module] = [];

		loadScripts();
    } else {
		_loadScript(module, jsObj, callback);
		moduleJs[module] = [module];
	}
}


function loadScripts(){
	if(index < jsArray.length){
		moduleJs[moduleName].push(moduleName + index);
		_loadScript(moduleName+index, jsArray[index++], loadScripts);
	} else {
		if(callbackFunc&&typeof(callbackFunc)=="function"){ 
			callbackFunc();
		}
	}
}

function _loadScript(id, url, callback) { 
	var c = document.getElementsByTagName("body")[0];
	var d = document.createElement("script");
	d.type = "text/javascript";
	d.src = url;
	d.id = id;
	d.onload = callback;
	c.appendChild(d);
}