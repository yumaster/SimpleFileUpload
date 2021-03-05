/**简单图片上传按钮
*@author yushuai_w
*/
function _init_FileUpload(_requestUrl) {
    var $ = jQuery,
        $list = $('#fileList'),
        // Web Uploader实例
        simple_upload;
    // 初始化Web Uploader
    simple_upload = WebUploader.create({
        // 选完文件后，是否自动上传。
        auto: true,
        // swf文件路径
        swf: '../vendors/webuploader-0.1.5/Uploader.swf',
        // 文件接收服务端。
        server: typeof (_requestUrl) == 'undefined' ? '/Upload/UploadFile' : _requestUrl,
        // 选择文件的按钮。可选。
        // 内部根据当前运行时创建，可能是input元素，也可能是flash.
        pick: '#uploadFile',

        // 只允许选择图片文件。
        accept: {
            title: 'Files',
            extensions: 'doc,docx,xls,xlsx,ppt,pptx',
            mimeTypes: 'application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-powerpoint,application/vnd.openxmlformats-officedocument.presentationml.presentation'
        }
    });
    // 当有文件添加进来的时候
    simple_upload.on('fileQueued', function (file) {

        //add by wys 添加列表样式类
        if (!$list.hasClass('wu-example')) {
            $list.addClass('wu-example');
        }

        var $li = $(
            '<div id="' + file.id + '" class="">' +
            //'<img>' +
            '<div class="info">' + file.name + '</div>' +
            '</div>'
        );
        // $list为容器jQuery实例
        $list.append($li);
    });

    // 文件上传过程中创建进度条实时显示。
    simple_upload.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress span');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<p class="progress"><span></span></p>')
                .appendTo($li)
                .find('span');
        }
        $percent.css('width', percentage * 100 + '%');
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    simple_upload.on('uploadSuccess', function (file) {
        $('#' + file.id).addClass('upload-state-done');
    });

    // 文件上传失败，显示上传出错。
    simple_upload.on('uploadError', function (file) {
        var $li = $('#' + file.id),
            $error = $li.find('div.error');

        // 避免重复创建
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }

        $error.text('上传失败');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    simple_upload.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
}