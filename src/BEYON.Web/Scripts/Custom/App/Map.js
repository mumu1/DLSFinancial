var viewer;
$(document).ready(function () {
	 //基础底图
    var baseImageProvider = new BeyonMap.UrlTemplateImageryProvider({
        url: '/Content/beyonmap/Assets/Textures/NaturalEarthII/{z}/{x}/{reverseY}.jpg',
        tilingScheme: new BeyonMap.GeographicTilingScheme(),
        maximumLevel: 2
    })

    var terrainProvider = new BeyonMap.BeyonMapTerrainProvider({
        url: MapServerURL + 'data/jilindem',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/')
    });

    viewer = new BeyonMap.Viewer('beyonmapContainer', {
        imageryProvider: baseImageProvider,
        baseLayerPicker: false,
       terrainProvider: terrainProvider,
        animation: false,
        timeline: false,
        homeButton: false,
        geocoder: false,		 
        navigationHelpButton: false,
        fullscreenButton: false,
        layersButton: true,
        scene3DOnly: true,
        infoBox: true,

        contextOptions: { allowTextureFilterAnisotropic: false }
    });


    //---------------------- 地图数据加载 -------------------//
    var earthProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'wxglobal',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel : 0,
        maximumLevel: 6
    });
    viewer.layersButton.addLayer("卫星影像", earthProvider, true, true);

    var chinaProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'wxchina',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 7,
        maximumLevel: 9,
        rectangle: BeyonMap.Rectangle.fromDegrees(56.69704483695651, 0.2109374999999964, 152.83420516304346, 57.4453125)
    });
    viewer.layersButton.addLayer("卫星影像", chinaProvider, true, true);

    var provinceProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'wxjilinsheng',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 10,
        maximumLevel: 11,
        rectangle: BeyonMap.Rectangle.fromDegrees(121.1484375, 40.370537640207075, 131.9765625, 46.816962359792925)
    });
    viewer.layersButton.addLayer("卫星影像", provinceProvider, true, true);

    var cityProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'wxjilinshi',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 12,
        maximumLevel: 12,
        rectangle: BeyonMap.Rectangle.fromDegrees(125.07647970448366, 42.390667459239125, 129.13632600203803, 44.807659646739125)
    });
    viewer.layersButton.addLayer("卫星影像", cityProvider, true, true);

    var smallRangeProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'wxjilinshi',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 13,
        maximumLevel: 16,
        rectangle: BeyonMap.Rectangle.fromDegrees(126.5463843304178, 43.80963134765625, 126.6884789508322, 43.89422607421875)
    });
    viewer.layersButton.addLayer("卫星影像", smallRangeProvider, true, true);

    //---------------------- 电子地图数据加载 -------------------//
    var dzearthProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'dzglobal',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel : 0,
        maximumLevel: 6
    });
    viewer.layersButton.addLayer("电子地图", dzearthProvider, false, true);

    var dzchinaProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'dzchina',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 7,
        maximumLevel: 9,
        rectangle: BeyonMap.Rectangle.fromDegrees(56.69704483695651, 0.2109374999999964, 152.83420516304346, 57.4453125)
    });
    viewer.layersButton.addLayer("电子地图", dzchinaProvider, false, true);

    var dzprovinceProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'dzjilinsheng',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 10,
        maximumLevel: 11,
        rectangle: BeyonMap.Rectangle.fromDegrees(121.1484375, 40.370537640207075, 131.9765625, 46.816962359792925)
    });
    viewer.layersButton.addLayer("电子地图", dzprovinceProvider, false, true);

    var dzcityProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'dzjilinshi',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 12,
        maximumLevel: 12,
        rectangle: BeyonMap.Rectangle.fromDegrees(125.07647970448366, 42.390667459239125, 129.13632600203803, 44.807659646739125)
    });
    viewer.layersButton.addLayer("电子地图", dzcityProvider, false, true);

    var dzsmallRangeProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'dzmin',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        //minimumLevel: 13,
        maximumLevel: 16,
        rectangle: BeyonMap.Rectangle.fromDegrees(126.5463843304178, 43.80963134765625, 126.6884789508322, 43.89422607421875)
    });
    viewer.layersButton.addLayer("电子地图", dzsmallRangeProvider, false, true);

    //---------------------- 高精度数据加载 -------------------//
    var dsmProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'DSM',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        maximumLevel: 20,
    });
    viewer.layersButton.addLayer("数字地表模型(DSM)", dsmProvider, false, true);

    //var dsmRBGProvider = new BeyonMap.GeoWebCacheImageryProvider({
    //    layer: 'DSM_RGB',
    //    url: MapServerURL + 'geowebcache/service/wmts',
    //    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
    //    maximumLevel: 20,
    //});
    //viewer.layersButton.addLayer("DSM RGB影像", dsmRBGProvider, false, true);

    var domProvider = new BeyonMap.GeoWebCacheImageryProvider({
        layer: 'DOM',
        url: MapServerURL + 'geowebcache/service/wmts',
        proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
        maximumLevel: 20,
    });
    viewer.layersButton.addLayer("数字正射影像(DOM)", domProvider, true, true);

    //var aspectProvider = new BeyonMap.GeoWebCacheImageryProvider({
    //    layer: 'aspect',
    //    url: MapServerURL + 'geowebcache/service/wmts',
    //    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
    //    maximumLevel: 20,
    //});
    //viewer.layersButton.addLayer("坡向", aspectProvider, false, true);

    //var slopeProvider = new BeyonMap.GeoWebCacheImageryProvider({
    //    layer: 'slope',
    //    url: MapServerURL + 'geowebcache/service/wmts',
    //    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
    //    maximumLevel: 20,
    //});
    //viewer.layersButton.addLayer("坡度", slopeProvider, false, true);

    //var contourProvider = new BeyonMap.GeoWebCacheImageryProvider({
    //    layer: 'contour',
    //    url: MapServerURL + 'geowebcache/service/wmts',
    //    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
    //    maximumLevel: 20,
    //});
    //viewer.layersButton.addLayer("等高线", contourProvider, false, true);

    //---------------------------------注记图层-------------------------------------------//
    //Add Address
    var labelsCity = new BeyonMap.LabelCollection();
    viewer.layersButton.addLayer("市镇注记", labelsCity, true, false);
    var countyCity = new BeyonMap.LabelCollection();
    viewer.layersButton.addLayer("市镇注记", countyCity, true, false);
    var labelsAddress = new BeyonMap.LabelCollection();
    viewer.layersButton.addLayer("乡村注记", labelsAddress, true, false);
    var labelsRoad = new BeyonMap.LabelCollection();
    //viewer.layersButton.addLayer("道路注记", labelsRoad, true, false);


  

    var cartesian3Scratch = new BeyonMap.Cartesian3();
    var cityAddress = BeyonMap.GeoJsonDataSource.load('../Content/data/city.geojson');
    cityAddress.then(function (dataSource) {
        //viewer.dataSources.add(dataSource);
        var entities = dataSource.entities.values;
        var nowtime = BeyonMap.JulianDate.now();

        for (var i = 0; i < entities.length; i++) {
            var entity = entities[i];
            var cartesian = entity.position.getValue(nowtime, cartesian3Scratch);
            var cartographic = viewer.scene.globe.ellipsoid.cartesianToCartographic(cartesian);

            if (entity.type == 'city') {
                cartographic.height += 800;
                cartesian = viewer.scene.globe.ellipsoid.cartographicToCartesian(cartographic);

                labelsCity.add({
                    position: cartesian,
                    text: entity.name,
                    font: '16px Microsoft YaHei',
                    fillColor: BeyonMap.Color.YELLOW,
                    outlineColor: BeyonMap.Color.BLACK,
                    outlineWidth: 2,
                    style: BeyonMap.LabelStyle.FILL_AND_OUTLINE,
                    translucencyByDistance: new BeyonMap.NearFarScalar(1e5, 0.7, 1e7, 0)
                });
            } else {
                cartographic.height += 300;
                cartesian = viewer.scene.globe.ellipsoid.cartographicToCartesian(cartographic);

                labelsCity.add({
                    position: cartesian,
                    text: entity.name,
                    font: '14px Microsoft YaHei',
                    fillColor: BeyonMap.Color.WHITE,
                    outlineColor: BeyonMap.Color.BLACK,
                    outlineWidth: 1,
                    style: BeyonMap.LabelStyle.FILL_AND_OUTLINE,
                    translucencyByDistance: new BeyonMap.NearFarScalar(2e4, 0.8, 5e5, 0)
                });
            }
        }
    }).otherwise(function (error) {
        //Display any errrors encountered while loading.
        window.alert(error);
    });

    var townAddress = BeyonMap.GeoJsonDataSource.load('../Content/data/village.geojson');
    townAddress.then(function (dataSource) {
        //viewer.dataSources.add(dataSource);
        var entities = dataSource.entities.values;
        var nowtime = BeyonMap.JulianDate.now();
        for (var i = 0; i < entities.length; i++) {
            var entity = entities[i];
            var cartesian = entity.position.getValue(nowtime, cartesian3Scratch);
            var cartographic = viewer.scene.globe.ellipsoid.cartesianToCartographic(cartesian);
            cartographic.height += 250;
            cartesian = viewer.scene.globe.ellipsoid.cartographicToCartesian(cartographic);

            labelsAddress.add({
                position: cartesian,
                text: entity.name,
                font: '13px Microsoft YaHei',
                fillColor: BeyonMap.Color.WHITE,
                outlineColor: BeyonMap.Color.BLACK,
                outlineWidth: 1,
                style: BeyonMap.LabelStyle.FILL_AND_OUTLINE,
                translucencyByDistance: new BeyonMap.NearFarScalar(1e4, 1, 3e4, 0)
            });
        }
    }).otherwise(function (error) {
        //Display any errrors encountered while loading.
        window.alert(error);
    });

    //var b = new BeyonMap.MarkerCollection(viewer);
    ////for (var i = 0; i < '@Model.Count'; i++) {

    ////    //106.752603，22.111146；106.755371，22.111115
    ////    var people1 = b.add("@Model"[i].jd, 22.53, '董明珠1213123', { id: 1, name: '董明珠' }, { callback: showProperties, labelFillColor: BeyonMap.Color.fromCssColorString('#FF0088') });
    ////}
    //drawingManager.drawPrimitives.add(b);

    navigationInitialization(viewer);

    viewer.enableInfoOrSelection = false;

    viewer.camera.flyTo({
        destination: BeyonMap.Cartesian3.fromDegrees(126.626, 43.84, 3000.0)
    });

    //fly to position
    viewer.scene.screenSpaceCameraController.minimumZoomDistance = 20;
});