function fileDownload(url, filename) {
            if ($('#BeyonDBFileDownload').length > 0) {
                $('#BeyonDBFileDownload').attr('action', url);
                $('#BeyonDBFileDownloadInput').attr('value', filename);
                $('#BeyonDBFileDownload').submit();
                return;
            }
            var form = $("<form>");   //定义一个form表单
            form.attr('style', 'display:none');   //在form表单中添加查询参数
            form.attr('target', '');
            form.attr('method', 'post');
            form.attr('action', url);
            form.attr('id', "BeyonDBFileDownload");

            var input1 = $('<input>');
            input1.attr('type', 'hidden');
            input1.attr('name', 'Filepath');
            input1.attr('value', filename);
            input1.attr('id', 'BeyonDBFileDownloadInput');
            $('body').append(form);  //将表单放置在web中 
            form.append(input1);   //将查询参数控件提交到表单上
            form.submit();
        }