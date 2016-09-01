(function ($) {
	var baseConfig = {
		topOffset: 40,
		bgColor: '#EEEEEE'
	};

	var $win;

	function processResizeColumns(table, header) {
		var headerCopy = table.find('thead.fixed-header-copy > tr:first')
		  .css('width', table.outerWidth())
		;

		$('tr:first > th', header).each(function (i, h) {
			headerCopy.find('th:eq(' + i + ')').css('width', $(h).outerWidth());
		});
	}

	function processScroll(table, header) {
		var scrollTop;
		var headTop;
		var isFixed;

		if (!table.is(':visible')) {
			return;
		}

		isFixed = header.data('isFixed');
		headTop = header.length && header.offset().top - header.data('topOffset');
		scrollTop = $win.scrollTop();

		if (scrollTop >= headTop && !isFixed) {
			isFixed = 1;
		} else if (scrollTop <= headTop && isFixed) {
			isFixed = 0;
		}

		isFixed ? $('thead.fixed-header-copy', table).removeClass('hide')
				: $('thead.fixed-header-copy', table).addClass('hide');

		header.data('isFixed', isFixed);
	}


	$.fn.fixedHeader = function (options) {
		options = $.extend({}, baseConfig, options || {});

		return this.each(function () {
			var table = $(this);
			var header = $('thead.fixed-header', table);
			var config;

			if (table.find('thead.fixed-header-copy').length || !header.length) {
				return;
			}

			table.addClass('table-fixed-header');

			config = $.extend({
				topOffset: table.data('fixed-header-top-offset'),
				bgColor: table.data('fixed-header-bg-color')
			}, options);

			// hack sad times - holdover until rewrite for 2.1
			header.on('click', function () {
				if (!header.data('isFixed')) {
					setTimeout(function () {
						$win.scrollTop($win.scrollTop() - 47); // not sure what this is for (yr)
					}, 10);
				}
			}).data('topOffset', config.topOffset);

			header.clone()
			  .removeClass('fixed-header')
			  .addClass('fixed-header-copy')
			  .css({
				  position: 'fixed',
				  margin: '0 auto',
				  top: config.topOffset + 'px',
				  backgroundColor: config.bgColor,
				  zIndex: 1020    /* 10 less than .navbar-fixed to prevent any overlap */
			  })
			  .appendTo(table);

			processResizeColumns(table, header);
			processScroll(table, header);
		});
	};


	$(function () {
		$win = $(window).on('resize', function () {
			$('table.table-fixed-header').each(function () {
				processResizeColumns($(this), $('thead.fixed-header', this));
			});
		}).on('scroll', function () {
			$('table.table-fixed-header').each(function () {
				processScroll($(this), $('thead.fixed-header', this));
			});
		});

		// auto set on domcument ready
		$('table.table-fixed-header').fixedHeader();
	});
})(jQuery);