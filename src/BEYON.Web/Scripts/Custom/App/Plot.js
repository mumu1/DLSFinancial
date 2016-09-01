var drawingManager;

$(document).ready(function () {

	 drawingManager = new BeyonMap.DrawingManager(viewer, 
		{
			drawingControl : false, 
			isOpen : true,
			drawingControlOptions : {
				drawingModes : [
					
					BeyonMap.DrawingTypes.DRAWING_MARKER,
					BeyonMap.DrawingTypes.DRAWING_POLYLINE,
					BeyonMap.DrawingTypes.DRAWING_POLYGON,

					BeyonMap.DrawingTypes.DRAWING_ATTACK_ARROW,
					BeyonMap.DrawingTypes.DRAWING_TAILED_ATTACK_ARROW,
					BeyonMap.DrawingTypes.DRAWING_SQUAD_COMBAT,
					BeyonMap.DrawingTypes.DRAWING_TAILED_SQUAD_COMBAT,
					BeyonMap.DrawingTypes.DRAWING_DOUBLE_ARROW,

					BeyonMap.DrawingTypes.DRAWING_MARKER_QUERY,
					BeyonMap.DrawingTypes.DRAWING_RECTANGLE_QUERY,
					BeyonMap.DrawingTypes.DRAWING_CIRCLE_QUERY,

					BeyonMap.DrawingTypes.DRAWING_CLEAR
			]},
			markerOptions : {
				scale : 1,
				color : new BeyonMap.Color(1.0, 1.0, 1.0, 1.0),
				editable : true
			},
			polylineOptions : {
				material : BeyonMap.Material.fromType('Color', {
					color : new BeyonMap.Color(1.0, 1.0, 0.0, 1.0)
				}),
				editable : true
			},
			polygonOptions : {
				material : BeyonMap.Material.fromType('Color', {
					color : new BeyonMap.Color(1.0, 0.0, 0.0, 1.0)
				}),
				editable : true
			},
			arrowOptions : {
				material : BeyonMap.Material.fromType('Color', {
					color : new BeyonMap.Color(1.0, 1.0, 0.0, 1.0)
				}),
				editable : true
			},
			rectangleOptions : {
				query : true
			},
			circleOptions : {
				query : true
			}
	});
	 viewer.pickPrimitiveEvent.addEventListener(function (primitive) {
	     //var sd = primitive.getType();
	     //primitive.setEditable(true);
	     var id = primitive.properties.id;
	     var index = "_" + id;
	     $('#window').load('../Map/Plot/PointModel',
           { index: index }  //自己定义需要传入的数据
        );
	     //var ids = primitive.id;
	 });


	 


	 drawingManager.dragEndEvent.addEventListener(function (primitive) {
	     //alert("great");
	     //alert(primitive);
	 });

    //获取线段绘制完成事件
    //1.添加标绘点信息
	 window.newpoint;
	 window.newrect;
	 window.temp;
	 drawingManager.addEventListener("AddPlotPointComplete", function (type, object) {

	     window.temp = object;
	     var long = BeyonMap.Math.toDegrees(object.positions[0].longitude);
	     var lati = BeyonMap.Math.toDegrees(object.positions[0].latitude);

	     //var la = object.positions[0].latitude.toString();
	     //var lo = object.positions[0].longitude.toString();
	     var location = lati.toString()+ ";" + long.toString()+"-point";
	     //object.primitive.setEditable(true);
	     object.primitive.showInfo = false;
	     var index = location + "_";
	     //弹出对话框
	     $('#window').load('../Map/Plot/PointModel',
            { index: index }  //自己定义需要传入的数据
         );
	 });

	 drawingManager.addEventListener("AddPlotRectComplete", function (type, object) {
	     window.temp = object;
	     var la = BeyonMap.Math.toDegrees(object.positions[0].latitude).toString();
	     var lo = BeyonMap.Math.toDegrees(object.positions[0].longitude).toString();
	     var location = la + ";" + lo;
	     var la = BeyonMap.Math.toDegrees(object.positions[1].latitude).toString();
	     var lo = BeyonMap.Math.toDegrees(object.positions[1].longitude).toString();
	     location =location+":"+ la + "|" + lo+"-rect";
	     //drawingManager.drawPrimitives.add(object.primitive);
	     //object.primitive.setEditable(true);
	     var index = location + "_";
	     //弹出对话框
	     $('#window').load('../Map/Plot/PointModel',
            { index: index }  //自己定义需要传入的数据
         );
	 });

	 drawingManager.addEventListener("AddPlotCircleComplete", function (type, object) {
	     drawingManager.drawPrimitives.add(object.primitive);
	     //object.primitive.setEditable(true);
	     //弹出对话框
	     $('#window').load('../Map/Plot/CircleModel',
            {}  //自己定义需要传入的数据
         );
	 });

	 drawingManager.addEventListener("AddPlotPolygonComplete", function (type, object) {
	     //object.primitive.setEditable(true);
	     //弹出对话框
	     $('#window').load('../Map/Plot/PolygonModel',
            {}  //自己定义需要传入的数据
         );
	 });
});

var b = new BeyonMap.MarkerCollection(viewer);



var plotbeforeop = {
    //direction : BeyonMap.Direction.BOTTOM, 
    font: '14px Microsoft YaHei',
    url: '../Content/img/plot/plotbefore.png',
    selectUrl: '../Content/img/plot/plotbefore.png',
    scale: 0.6,
    showInfo: false,
    labelBackgroundColor: '#51A831'
}
var plotafterop = {
    //direction : BeyonMap.Direction.BOTTOM, 
    font: '14px Microsoft YaHei',
    url: '../Content/img/plot/plotafter_tosubmit.png',
    selectUrl: '../Content/img/plot/plotafter_tosubmit.png',
    scale: 0.6,
    showInfo: false,
    labelBackgroundColor: '#51A831'
}
var submitrop = {
    //direction : BeyonMap.Direction.BOTTOM, 
    font: '14px Microsoft YaHei',
    url: '../Content/img/plot/plotafter_auditing.png',
    selectUrl: '../Content/img/plot/plotafter_auditing.png',
    scale: 0.6,
    showInfo: false,
    labelBackgroundColor: '#51A831'
}
var auditerop = {
    //direction : BeyonMap.Direction.BOTTOM, 
    font: '14px Microsoft YaHei',
    url: '../Content/img/plot/plotafter_pass.png',
    selectUrl: '../Content/img/plot/plotafter_pass.png',
    scale: 0.6,
    showInfo: false,
    labelBackgroundColor: '#51A831'
}
var backop = {
    //direction : BeyonMap.Direction.BOTTOM, 
    font: '14px Microsoft YaHei',
    url: '../Content/img/plot/plotafter_back.png',
    selectUrl: '../Content/img/plot/plotafter_back.png',
    scale: 0.6,
    showInfo: false,
    labelBackgroundColor: '#51A831'
}

//var b = new BeyonMap.MarkerCollection(viewer);
var actionUrl = "../Map/Plot/GetPlots";
$.ajax({
    type: "POST",
    url: actionUrl,
    success: function (result) {
        if (result.ResultType === 0) {
            //var a = "[{\"latitude\":43.851172385824306,\"longitude\":126.62219285149304,\"markercode\": \"1214-plotafter\",\"name\":\"23\"},{\"latitude\":43.84269392455245,\"longitude\":126.62915825757153,\"markercode\": \"123-after\",\"name\":\"12\"}]";
            var jsonPoint = eval("(" + result.Message.substr(0, result.Message.lastIndexOf("|")) + ")");
            for (var i = 0; i < jsonPoint.length; i++) {
                var markerid = jsonPoint[i].markercode.substr(jsonPoint[i].markercode.lastIndexOf("-") + 1, 5);
                switch (jsonPoint[i].markercode.substr(jsonPoint[i].markercode.lastIndexOf("-") + 1, 5))
                {
                    case "befor":
                        var point = b.add(jsonPoint[i].longitude, jsonPoint[i].latitude, jsonPoint[i].name, { id: jsonPoint[i].markercode, name: jsonPoint[i].name }, plotbeforeop);
                        break;
                    case "after":
                        var point = b.add(jsonPoint[i].longitude, jsonPoint[i].latitude, jsonPoint[i].name, { id: jsonPoint[i].markercode, name: jsonPoint[i].name }, plotafterop);
                        break;
                    case "submi":
                        var point = b.add(jsonPoint[i].longitude, jsonPoint[i].latitude, jsonPoint[i].name, { id: jsonPoint[i].markercode, name: jsonPoint[i].name }, submitrop);
                        break;
                    case "audit":
                        var point = b.add(jsonPoint[i].longitude, jsonPoint[i].latitude, jsonPoint[i].name, { id: jsonPoint[i].markercode, name: jsonPoint[i].name }, auditerop);
                        break;
                    case "backk":
                        var point = b.add(jsonPoint[i].longitude, jsonPoint[i].latitude, jsonPoint[i].name, { id: jsonPoint[i].markercode, name: jsonPoint[i].name }, backop);
                        break;

                }
            }
            var jsonRect = eval("(" + result.Message.substr(result.Message.lastIndexOf("|")+1,result.Message.length) + ")");
            for (var i = 0; i < jsonRect.length; i++) {
                var markerid = jsonRect[i].markercode.substr(jsonRect[i].markercode.lastIndexOf("-") + 1, 5);
                switch (jsonRect[i].markercode.substr(jsonRect[i].markercode.lastIndexOf("-") + 1, 5))
                {
                    case "befor":
                        var plot = BeyonMap.PolygonPrimitive.fromDegrees([jsonRect[i].longitude1, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude2, 0, jsonRect[i].longitude1, jsonRect[i].latitude2, 0], rectbrforeoption);
                        plot.properties = BeyonMap.defaultValue( plot.properties, {});
                        plot.properties.id = jsonRect[i].markercode;
                        drawingManager.drawPrimitives.add(plot);
                        break;
                    case "after":
                        var plot = BeyonMap.PolygonPrimitive.fromDegrees([jsonRect[i].longitude1, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude2, 0, jsonRect[i].longitude1, jsonRect[i].latitude2, 0], rectafteroption);
                        plot.properties = BeyonMap.defaultValue(plot.properties, {});
                        plot.properties.id = jsonRect[i].markercode;
                        drawingManager.drawPrimitives.add(plot);
                        break;
                    case "submi":
                        var plot = BeyonMap.PolygonPrimitive.fromDegrees([jsonRect[i].longitude1, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude2, 0, jsonRect[i].longitude1, jsonRect[i].latitude2, 0], rectsubmitoption);
                        plot.properties = BeyonMap.defaultValue(plot.properties, {});
                        plot.properties.id = jsonRect[i].markercode;
                        drawingManager.drawPrimitives.add(plot);
                        break;
                    case "audit":
                        var plot = BeyonMap.PolygonPrimitive.fromDegrees([jsonRect[i].longitude1, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude2, 0, jsonRect[i].longitude1, jsonRect[i].latitude2, 0], rectauditoption);
                        plot.properties = BeyonMap.defaultValue(plot.properties, {});
                        plot.properties.id = jsonRect[i].markercode;
                        drawingManager.drawPrimitives.add(plot);
                        break;
                    case "backk":
                        var plot = BeyonMap.PolygonPrimitive.fromDegrees([jsonRect[i].longitude1, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude1, 0, jsonRect[i].longitude2, jsonRect[i].latitude2, 0, jsonRect[i].longitude1, jsonRect[i].latitude2, 0], rectbackoption);
                        plot.properties = BeyonMap.defaultValue(plot.properties, {});
                        plot.properties.id = jsonRect[i].markercode;
                        drawingManager.drawPrimitives.add(plot);
                        break;
                }              
            }
        }
        else {
            toastr.error(result.Message);
        }
    },
    error: function () {
        toastr.error('网络错误，请重新提交！');
    }
});
drawingManager.drawPrimitives.add(b);


var rectbrforeoption = { material: BeyonMap.Material.fromType('Color', { color: BeyonMap.Color.fromAlpha(BeyonMap.Color.AQUA,0.6) }), };
var rectafteroption = { material: BeyonMap.Material.fromType('Color', { color: BeyonMap.Color.fromAlpha(BeyonMap.Color.WHITESMOKE,0.6) }), };
var rectsubmitoption = { material: BeyonMap.Material.fromType('Color', { color: BeyonMap.Color.fromAlpha(BeyonMap.Color.DEEPSKYBLUE,0.6) }), };
var rectauditoption = { material: BeyonMap.Material.fromType('Color', { color: BeyonMap.Color.fromAlpha(BeyonMap.Color.LAWNGREEN,0.6) }), };
var rectbackoption = { material: BeyonMap.Material.fromType('Color', { color: BeyonMap.Color.fromAlpha(BeyonMap.Color.ORANGERED,0.6) }), };

//var rectbrforeoption = { material: BeyonMap.Material.fromType('Color', { color: new BeyonMap.Color(1, 0.94, 0, 0.5) }), };
//var rectafteroption = { material: BeyonMap.Material.fromType('Color', { color: new BeyonMap.Color(1.0, 1.0, 0.0, 1.0) }), };
//var rectsubmitoption = { material: BeyonMap.Material.fromType('Color', { color: new BeyonMap.Color(0.6196, 0.8431, 0.1607, 1.0) }), };
//var rectauditoption = { material: BeyonMap.Material.fromType('Color', { color: new BeyonMap.Color(1.0, 1.0, 0.0, 1.0) }), };
//var rectbackoption = { material: BeyonMap.Material.fromType('Color', { color: new BeyonMap.Color(1.0, 1.0, 0.0, 1.0) }), };
