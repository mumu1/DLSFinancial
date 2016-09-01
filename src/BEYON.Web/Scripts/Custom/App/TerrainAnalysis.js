$(document).ready(function () {
    switchAnaylyTerrain($('#mapType .mapTypeCard.active').data('name'));

    //设定点击事件
    $('#mapType .mapTypeCard').unbind('click');
    $('#mapType .mapTypeCard').click(function (event) {
        $('#mapType .mapTypeCard').removeClass('active');
        $(event.target).addClass('active');
        switchAnaylyTerrain($(event.target).data('name'));
    });

    //关闭分析按钮
    $('#mapType .closeMapType').unbind('click');
    $('#mapType .closeMapType').click(function () {
        //关闭图层Todo
        $('#MapTerrainTheme').css('display', 'none');
        switchAnaylyTerrain('');
    });
});

/**
 * 根据图层名称，显示图层
 */
function showLayer(name, show) {
    switch (name) {
        case '坡向':
            if (!window.layerAspect) {
                var aspectProvider = new BeyonMap.GeoWebCacheImageryProvider({
                    layer: 'aspect',
                    url: MapServerURL + 'geowebcache/service/wmts',
                    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
                    maximumLevel: 20,
                });
                window.layerAspect = viewer.scene.imageryLayers.addImageryProvider(aspectProvider);
            }
            window.layerAspect.show = show;
            break;
        case '坡度':
            if (!window.layerSlope) {
                var slopeProvider = new BeyonMap.GeoWebCacheImageryProvider({
                    layer: 'slope',
                    url: MapServerURL + 'geowebcache/service/wmts',
                    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
                    maximumLevel: 20,
                });
                window.layerSlope = viewer.scene.imageryLayers.addImageryProvider(slopeProvider);
            }
            window.layerSlope.show = show;
            break;
        case '等高线':
            if (!window.layerContour) {
                var contourProvider = new BeyonMap.GeoWebCacheImageryProvider({
                    layer: 'contour',
                    url: MapServerURL + 'geowebcache/service/wmts',
                    proxy: new BeyonMap.BeyonDBProxy('/Common/Home/Proxy/'),
                    maximumLevel: 20,
                });
                window.layerContour = viewer.scene.imageryLayers.addImageryProvider(contourProvider);
            }
            window.layerContour.show = show;
            break;
    }
}

function switchAnaylyTerrain(name) {
    switch (name) {
        case 'slope':
            $('#MapLegend').css('background-image', 'url(../Content/img/terrain/slope_legend_247x322.png)');
            $('#MapLegend').css('display', 'block');
            showLayer('坡向', false);
            showLayer('等高线', false);
            showLayer('坡度', true);

            break;
        case 'aspect':
            $('#MapLegend').css('background-image', 'url(../Content/img/terrain/aspect_legend_116x322.png)');
            $('#MapLegend').css('display', 'block');

            showLayer('坡度', false);
            showLayer('等高线', false);
            showLayer('坡向', true);
            break;
        case 'contour':
            $('#MapLegend').css('display', 'none');
            showLayer('坡度', false);
            showLayer('坡向', false);
            showLayer('等高线', true);
            break;
        default:
            showLayer('坡向', false);
            showLayer('等高线', false);
            showLayer('坡度', false);
            break;
    }
}