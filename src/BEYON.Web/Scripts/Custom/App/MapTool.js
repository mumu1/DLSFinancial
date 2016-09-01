$(document).ready(function () {
	 //工具栏点击事件
    $('.btn-ribbon').click(function (e) {
		 if(e.target.id !== undefined){
			 if (typeof(eval(e.target.id)) == "function") {
				window[e.target.id](); 
			 }
		 }
	 });
});

/**
* 执行点标绘事件
**/
function toolPlotPoint() {
    drawingManager.startDrawingMarker({
        'customId': 'AddPlotPoint',
        'cursorUrl': "../Content/img/plot/plotPoint.png",
        '_name': "new",
    });
}
/**
*取消标绘
**/
function toolStopPlot() {
    drawingManager.stopDrawing();
}

/**
* 执行矩形标绘事件
**/
function toolPlotRect() {
    drawingManager.startDrawingRectangle({ 'customId': 'AddPlotRect' });
}

/**
* 执行圆标绘事件
**/
function toolPlotCircle() {
    drawingManager.startDrawingCircleQuery({ 'customId': 'AddPlotCircle' });
}

/**
* 执行多边形标绘事件
**/
function toolPlotPolygon() {
    drawingManager.startDrawingPolygon({ 'customId': 'AddPlotPolygon' });
}

/**
* 距离量算
**/
function toolDistance() {
    drawingManager.startDrawingDistance({});
}

/**
* 面积量算
**/
function toolArea() {
    drawingManager.startDrawingArea({});
}

/**
* 删除事件
**/
function toolClear() {
    for (var i = 0; i < drawingManager.drawPrimitives.length; i++) {
        var primitive = drawingManager.drawPrimitives.get(i);
        if (BeyonMap.defined(primitive) && primitive.isDestroyed && !primitive.isDestroyed()) {
            //点设置
            if (BeyonMap.defined(primitive.length)) {
                for (var j = 0; j < primitive.length; j++) {
                    var marker = primitive.get(j);
                    if (BeyonMap.defined(marker) && !BeyonMap.defined(marker.properties.id) && BeyonMap.defined(marker.markerCollection)) {
                        marker.markerCollection.remove(marker);
                    }
                }
            } else {
                if (!BeyonMap.defined(primitive.properties.id)) {
                    drawingManager.drawPrimitives.removeAndDestroy(primitive);
                }
            }
        }
    }
}

/**
* 高程量算
**/
function  toolHeight() {
    drawingManager.startDrawingHeight({});
}