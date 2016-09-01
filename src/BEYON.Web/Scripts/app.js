function loadScript(a, b) { 
	if (jsArray[a]) 
		b && (debugState && root.root.console.log("This script was already loaded %c: " + a, debugStyle_warning), b()); 
	else { 
		jsArray[a] = !0; 
		var c = document.getElementsByTagName("body")[0], d = document.createElement("script"); 
		d.type = "text/javascript", d.src = a, d.onload = b, c.appendChild(d) 
	} 
}

function checkURL() {
    var a = location.href.split("#").splice(1).join("#");
    if (!a)
        try {
            var b = window.document.URL;
            b && b.indexOf("#", 0) > 0 && b.indexOf("#", 0) < b.length + 1 && (a = b.substring(b.indexOf("#", 0) + 1))
        } catch (c) { }
    if (container = $("#content"), a) {
        $("nav li.active").removeClass("active"), $('nav li:has(a[href="' + a + '"])').addClass("active");
        var d = $('nav a[href="' + a + '"]').attr("title");
        document.title = d || document.title, debugState && root.console.log("Page title: %c " + document.title, debugStyle_green);
        loadURL(a + location.search, container);
    } else {
        var e = $('nav > ul > li:first-child > a[href!="#"]');
        window.location.hash = e.attr("href"), e = null
    }
}

function loadURL(url, container) {
    debugState && root.root.console.log("Loading URL: %c" + url, debugStyle);

    $.ajax({
        "type": "GET",
        "url": url,
        "dataType": "html",
        "cache": !0,
        "beforeSend": function () {
            if ($.navAsAjax && $(".google_maps")[0] && container[0] == $("#content")[0]) {
                var a = $(".google_maps"), c = 0;
                a.each(function () {
                    c++;
                    var b = document.getElementById(this.id);
                    c == a.length + 1 || (b && b.parentNode.removeChild(b), debugState && root.console.log("Destroying maps.........%c" + this.id, debugStyle_warning))
                }),
                debugState && root.console.log("\u2714 Google map instances nuked!!!")
            }

            if ($.navAsAjax && $(".dataTables_wrapper")[0] && container[0] == $("#content")[0]) {
                var d = $.fn.dataTable.fnTables(!0);
                $(d).each(function () {
                    0 != $(this).find(".details-control").length ? ($(this).find("*").addBack().off().remove(), $(this).dataTable().fnDestroy()) : $(this).dataTable().fnDestroy()
                }),
                debugState && root.console.log("\u2714 Datatable instances nuked!!!")
            }

            if ($.navAsAjax && $.intervalArr.length > 0 && container[0] == $("#content")[0] && enableJarvisWidgets) {
                for (; $.intervalArr.length > 0;)
                    clearInterval($.intervalArr.pop());
                debugState && root.console.log("\u2714 All JarvisWidget intervals cleared")
            }

            if ($.navAsAjax && container[0] == $("#content")[0] && enableJarvisWidgets && $("#widget-grid")[0] && ($("#widget-grid").jarvisWidgets("destroy"), debugState && root.console.log("\u2714 JarvisWidgets destroyed")), $.navAsAjax && container[0] == $("#content")[0]) {
                if ("function" == typeof pagedestroy)
                try {
                    pagedestroy(), debugState && root.console.log("\u2714 Pagedestroy()")
                } catch (e) {
                    pagedestroy = void 0, debugState && root.console.log("! Pagedestroy() Catch Error")
                }
                $.fn.sparkline && $("#content .sparkline")[0] && ($("#content .sparkline").sparkline("destroy"), debugState && root.console.log("\u2714 Sparkline Charts destroyed!")),
                $.fn.easyPieChart && $("#content .easy-pie-chart")[0] && ($("#content .easy-pie-chart").easyPieChart("destroy"), debugState && root.console.log("\u2714 EasyPieChart Charts destroyed!")),
                $.fn.select2 && $("#content select.select2")[0] && ($("#content select.select2").select2("destroy"), debugState && root.console.log("\u2714 Select2 destroyed!")),
                $.fn.mask && $("#content [data-mask]")[0] && ($("#content [data-mask]").unmask(), debugState && root.console.log("\u2714 Input Mask destroyed!")),
                $.fn.datepicker && $("#content .datepicker")[0] && ($("#content .datepicker").off(), $("#content .datepicker").remove(), debugState && root.console.log("\u2714 Datepicker destroyed!")),
                $.fn.slider && $("#content .slider")[0] && ($("#content .slider").off(), $("#content .slider").remove(), debugState && root.console.log("\u2714 Bootstrap Slider destroyed!"))
            }

            pagefunction = null,
            container.removeData().html(""),
            container.html('<h1 class="ajax-loading-animation"><i class="fa fa-cog fa-spin"></i> 加载中...</h1>'),
            container[0] == $("#content")[0] && ($("body").find("> *").filter(":not(" + ignore_key_elms + ")").empty().remove(),
            drawBreadCrumb(),
            $("html").animate({ "scrollTop": 0 }, "fast"))
        },
        "success": function (result) {
            container.css({ "opacity": "0.0" }).html(result).delay(50).animate({ "opacity": "1.0" }, 300),
            result = null,
            container = null
        },
        "error": function (c, d, e, f) {
            container.html('<h4 class="ajax-loading-error"><i class="fa fa-warning txt-color-orangeDark"></i> Error requesting <span class="txt-color-red">' + url + "</span>: " + c.status + ' <span style="text-transform: capitalize;">' + e + "</span></h4>")
        },
        "async": !0
    })
}

function drawBreadCrumb(a) { var b = $("nav li.active > a"), c = b.length; bread_crumb.empty(), bread_crumb.append($("<li>主页</li>")), b.each(function () { bread_crumb.append($("<li></li>").html($.trim($(this).clone().children(".badge").remove().end().text()))), --c || (document.title = bread_crumb.find("li:last-child").text()) }), void 0 != a && $.each(a, function (a, b) { bread_crumb.append($("<li></li>").html(b)), document.title = bread_crumb.find("li:last-child").text() }) }

function pageSetUp() { "desktop" === thisDevice ? ($("[rel=tooltip], [data-rel=tooltip]").tooltip(), $("[rel=popover], [data-rel=popover]").popover(), $("[rel=popover-hover], [data-rel=popover-hover]").popover({ "trigger": "hover" }), setup_widgets_desktop(), runAllCharts(), runAllForms()) : ($("[rel=popover], [data-rel=popover]").popover(), $("[rel=popover-hover], [data-rel=popover-hover]").popover({ "trigger": "hover" }), runAllCharts(), setup_widgets_mobile(), runAllForms()) }

function getParam(a) { a = a.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]"); var b = "[\\?&]" + a + "=([^&#]*)", c = new RegExp(b), d = c.exec(window.location.href); return null == d ? "" : d[1] }

$(document).on("click", 'nav a[href="#"]', function (a) { 
	a.preventDefault() }
)

$(window).on("hashchange", function () { 
	checkURL();
})

$(document).on("click", 'nav a[href]', function (a) { 
	var target = a.target;
	a.preventDefault() }
)






