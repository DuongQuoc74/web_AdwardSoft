var checkImg = "";
function CheckUrlPaster(url) {
    return new Promise(function (resolve, reject) {
        checkImg = "";
        var res = resolve;
        var part2 = url.substring(url.lastIndexOf('.'));
        var part1 = url.substr(0, url.lastIndexOf('.'));
        var mdpi = part1 + "-mdpi" + part2;
        var hdpi = part1 + "-hdpi" + part2;
        var xhdpi = part1 + "-xhdpi" + part2;
        var xxhdpi = part1 + "-xxhdpi" + part2;
        var xxxhdpi = part1 + "-xxxhdpi" + part2;

        var p0 = testImage(url, "Đường dẫn không hợp lệ");
        var p1 = testImage(mdpi, " - mdpi");
        var p2 = testImage(hdpi, " - hdpi");
        var p3 = testImage(xhdpi, " - xhdpi");
        var p4 = testImage(xxhdpi, " - xxhdpi");
        var p5 = testImage(xxxhdpi, " - xxxhdpi");

        p0.then((result) => {
            if (result === null) {
                var pError = Promise.all([p1, p2, p3, p4, p5]).then((value) => {
                    console.log(value);

                    for (var i = 0; i < value.length; i++) {
                        if (value[i] != null) {
                            checkImg += value[i];
                        }
                    }

                    if (checkImg === "") {
                        checkImg = "Image đủ size";
                    }
                    else {
                        var error = "Image thiếu size ";
                        checkImg = error.concat(checkImg);
                    }
                });

                pError.then(() => {
                    resolve(checkImg);
                })
            }
            else {
                resolve(result);
            }
        })
    });
}

function testImage(url, extension, timeout) {
    return new Promise(function (resolve, reject) {
        timeout = timeout || 600000;
        var timedOut = false, timer;
        var img = new Image();
        img.onerror = img.onabort = function () {
            if (!timedOut) {
                clearTimeout(timer);
                //callback(url, "error", extension);
                resolve(extension);
            }
        };
        img.onload = function () {
            if (!timedOut) {
                clearTimeout(timer);
                //callback(url, "success", extension);
                resolve(null);
            }
        };
        img.src = url;
        timer = setTimeout(function () {
            timedOut = true;
            img.src = "//!!!!/test.jpg";
            //callback(url, "timeout", extension);
            resolve(extension);
        }, timeout);
    });
}

function FormatCurrency(value) {
    value = String(value);
    var decimal = "";
    if (value.indexOf(".") > -1) {
        Number(value.substring(value.indexOf(".") + 1)) > 0 ? decimal = value.substring(value.indexOf(".")) : decimal = "";
        value = value.substring(0, value.indexOf("."));
    }
    var val = "";
    for (let i = value.length - 1; i >= 0; i--) {
        if (val.replace(/[,]/g, '').length % 3 === 0 && val.length > 0)
            val = value.charAt(i) + ',' + val;
        else
            val = value.charAt(i) + val;
    }
    if (val.startsWith(','))
        val = val.substring(1);
    return val + decimal;
}

function FormatCurrency(value, suffix = ',') {
    value = String(value);
    var decimal = "";
    if (value.indexOf(".") > -1) {
        Number(value.substring(value.indexOf(".") + 1)) > 0 ? decimal = value.substring(value.indexOf(".")) : decimal = "";
        value = value.substring(0, value.indexOf("."));
    }
    var val = "";
    for (let i = value.length - 1; i >= 0; i--) {
        var pattern = '[' + suffix + ']'
        var regex = new RegExp(pattern, 'g')
        if (val.replace(regex, '').length % 3 === 0 && val.length > 0)
            val = value.charAt(i) + suffix + val;
        else
            val = value.charAt(i) + val;
    }
    if (val.startsWith(','))
        val = val.substring(1);
    return val + decimal;
}

$(document).on("input", ".format-Currency", function () {
    var number = /^\d+$/;
    if (number.test($(this).val().replace(/[,]/g, ''))) {
        var path = /^\d{1,12}(?:\.\d{0,2})?$/;
        if (path.test($(this).val().replace(/[,]/g, ''))) {
            var value = $(this).val().replace(/[,]/g, '');
            var decimal = "";
            if (value.indexOf(".") > -1) {
                decimal = value.substring(value.indexOf("."));
                value = value.substring(0, value.indexOf("."));
            }
            var val = "";
            for (let i = value.length - 1; i >= 0; i--) {
                if (val.replace(/[,]/g, '').length % 3 === 0 && val.length > 0)
                    val = value.charAt(i) + ',' + val;
                else
                    val = value.charAt(i) + val;
            }
            if (val.startsWith(','))
                val = val.substring(1);
            return $(this).val(val + decimal);
        }
        else
            $(this).val($(this).val().slice(0, -1));
    }
    else {
        $(this).val($(this).val().slice(0, -1));
    }
}).on("change", ".format-Currency", function () {
    $(this).val(FormatCurrency($(this).val().replace(/[,]/g, '')));
});

$(document).on("input", ".format-number", function () {
    var number = /^-?\d{1,12}?$/;
    var max = $(this).attr("max") !== undefined ? parseFloat($(this).attr("max")) : 0;
    var min = $(this).attr("min") !== undefined ? parseFloat($(this).attr("min")) : 0;
    var negativeInteger = $(this).attr("negative-Integer") !== undefined ? true : false;
    if (negativeInteger) {
        if ($(this).val() == "-")
            return;
    }
    if ($(this).val().length == "") {
        $(this).val("0")
    }
    if (number.test($(this).val().replace(/[.]/g, ''))) {
        var path = /^-?\d{1,12}(?:\.\d{0,12})?$/;
        if (path.test($(this).val())) {
            if ($(this).val().startsWith("0") && $(this).val().length > 1)
                $(this).val($(this).val().substring(1));
            else {
                var val = parseFloat($(this).val());
                if (max > 0 || min > 0) {
                    if (min <= val)
                        $(this).val($(this).val());
                    else {
                        $(this).val($(this).val().slice(0, -1));
                        return;
                    }
                    if (val <= max)
                        $(this).val($(this).val());
                    else {
                        $(this).val($(this).val().slice(0, -1));
                        return;
                    }
                }
                else {
                    $(this).val($(this).val());
                }
            }
        }
        else {
            console.log($(this).val())
            $(this).val($(this).val().slice(0, -1));
        }
    }
    else {
        $(this).val($(this).val().slice(0, -1));
    }
});

$(document).on("input", ".format-phone", function () {
    var number = /^-?\d{1,12}?$/;
    var max = $(this).attr("max") !== undefined ? parseFloat($(this).attr("max")) : 0;
    var min = $(this).attr("min") !== undefined ? parseFloat($(this).attr("min")) : 0;
    var negativeInteger = $(this).attr("negative-Integer") !== undefined ? true : false;
    if (negativeInteger) {
        if ($(this).val() == "-")
            return;
    }
    if ($(this).val().length == "") {
        $(this).val("0")
    }
    if (number.test($(this).val().replace(/[.]/g, ''))) {
        var path = /^-?\d{1,12}(?:\.\d{0,12})?$/;
        if (path.test($(this).val())) {
            if ($(this).val().startsWith("0") && $(this).val().length > 1)
                $(this).val($(this).val().substring(1));
            else {
                var val = parseFloat($(this).val());
                if (max > 0 || min > 0) {
                    if (min <= val)
                        $(this).val($(this).val());
                    else {
                        $(this).val($(this).val().slice(0, -1));
                        return;
                    }
                    if (val <= max)
                        $(this).val($(this).val());
                    else {
                        $(this).val($(this).val().slice(0, -1));
                        return;
                    }
                }
                else {
                    $(this).val($(this).val());
                }
            }
        }
        else {
            console.log($(this).val())
            $(this).val($(this).val().slice(0, -1));
        }
    }
    else {
        $(this).val($(this).val().slice(0, -1));
    }
});

var rebindValidation = function () {
    $('form').each(function (i, f) {
        $form = $(f);
        $form.removeData('validator');
        $form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse($form);
    });
};

//$(window).on("load", function () {
//    var a = window.location.pathname;
//    var id = $("#MenuTable li").find("a[href='" + a + "']").attr("id");
//    $("#" + id).parents("li.headerLi").addClass("nav-item-open");
//    $("#" + id).parents("ul.nav-group-sub").css("display", "block");
//});

$(window).on("load", function () {
    $("#MenuTable").find("li.nav-item-open").parents("li").addClass("nav-item-open")
    $("#MenuTable").find("li.nav-item-open").parents("ul.nav-group-sub").css("display", "block");
})

//function initTINYMCE(id) {
//    tinymce.init({
//        selector: 'textarea#' + id,
//        plugins: 'print preview fullpage searchreplace autolink directionality fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime  lists wordcount  imagetools  help   ',
//        toolbar: 'formatselect | bold italic strikethrough forecolor backcolor | link image media  | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent | removeformat | addcomment | preview',
//        statusbar: false,
//        setup: function (editor) {
//            editor.on('change', function () {
//                tinymce.triggerSave();
//            });
//        }
//    });
//}

function initTINYMCE(id) {
    tinymce.remove('textarea#' + id)
    tinymce.init({
        selector: 'textarea#' + id,
        //plugins: 'print preview fullpage searchreplace autolink directionality advcode visualblocks visualchars fullscreen image link media mediaembed template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount tinymcespellchecker a11ychecker imagetools textpattern help formatpainter permanentpen pageembed tinycomments mentions linkchecker',
        plugins: 'code print preview fullpage searchreplace autolink directionality fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime  lists wordcount  imagetools  help   ',
        toolbar: 'code | fontselect | formatselect | bold italic strikethrough forecolor backcolor permanentpen formatpainter | link image media pageembed | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent | removeformat | addcomment',
        image_advtab: true,
        toolbar_sticky: true,
        images_upload_url: '/Image/Upload',
        relative_urls : false, 
        paste_data_images: true,
        remove_script_host: false, // disable remove host url
        content_css: [
            '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
            '/lib/tinymce/GoogleSans/stylesheet.css',
            '//www.tiny.cloud/css/codepen.min.css'
        ],
        link_list: [
            { title: 'My page 1', value: 'http://www.tinymce.com' },
            { title: 'My page 2', value: 'http://www.moxiecode.com' }
        ],
        image_list: [
            { title: 'My page 1', value: 'http://www.tinymce.com' },
            { title: 'My page 2', value: 'http://www.moxiecode.com' }
        ],
        image_class_list: [
            { title: 'None', value: '' },
            { title: 'Some class', value: 'class-name' }
        ],
        importcss_append: true,
        height: 400,
        templates: [
            { title: 'Some title 1', description: 'Some desc 1', content: 'My content' },
            { title: 'Some title 2', description: 'Some desc 2', content: '<div class="mceTmpl"><span class="cdate">cdate</span><span class="mdate">mdate</span>My content2</div>' }
        ],
        template_cdate_format: '[CDATE: %m/%d/%Y : %H:%M:%S]',
        template_mdate_format: '[MDATE: %m/%d/%Y : %H:%M:%S]',
        image_caption: true,
        spellchecker_dialog: true,
        spellchecker_whitelist: ['Ephox', 'Moxiecode'],
        tinycomments_mode: 'embedded',
        content_style: '.mce-annotation { background: #fff0b7; } .tc-active-annotation {background: #ffe168; color: black; }',
        font_formats: 'Google sans=Google sans;Lato=lato;Arial=arial;Helvetica=helvetica,Sans Serif=sans-serif;Times New Roman=Times New Roman;Tahoma=Tahoma;Roboto=roboto',
        setup: function (editor) {
            editor.on('change', function () {
                tinymce.triggerSave();
            });
        }
    });
}

function exportHtmlToWord(content, filename) {
    var header = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
        "xmlns:w='urn:schemas-microsoft-com:office:word' " +
        "xmlns='http://www.w3.org/TR/REC-html40'>" +
        "<head><meta charset='utf-8'><title>Export HTML to Word Document with JavaScript</title></head><body>";
    var footer = "</body></html>";
    var sourceHTML = header + content + footer;

    var source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
    var fileDownload = document.createElement("a");
    document.body.appendChild(fileDownload);
    fileDownload.href = source;
    fileDownload.download = filename;
    fileDownload.click();
    document.body.removeChild(fileDownload);
}

$(document).on("input", ".num2Text", function () {
    var number = /^\d+$/;
    if (number.test($(this).val().replace(/[,]/g, ''))) {
        var path = /^\d{1,21}(?:\.\d{0,2})?$/;
        if (path.test($(this).val().replace(/[,]/g, ''))) {
            var value = $(this).val().replace(/[,]/g, '');
            var decimal = "";
            if (value.indexOf(".") > -1) {
                decimal = value.substring(value.indexOf("."));
                value = value.substring(0, value.indexOf("."));
            }
            var val = "";
            for (let i = value.length - 1; i >= 0; i--) {
                if (val.replace(/[,]/g, '').length % 3 === 0 && val.length > 0)
                    val = value.charAt(i) + ',' + val;
                else
                    val = value.charAt(i) + val;
            }
            if (val.startsWith(','))
                val = val.substring(1);
            let target = "#" + $(this).data("inputtarget");
            if (value !== "0")
                $(target).val(ParseNumbertoString(val + decimal).replace(/  +/g, ' '));
            else
                $(target).val("Không đồng");
            $(this).val(val + decimal);
        }
        else
            $(this).val($(this).val().slice(0, -1));
    }
    else {
        $(this).val($(this).val().slice(0, -1));
    }
});

$(document).on("keyup", ".num2Text", function (e) {
    let target = "#" + $(this).data("inputtarget");
    if (e.keyCode == 8 && $(this).val() == "")
        $(target).val("");
});

let arrNumber = [" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ", " mười "]
let arrUnit = [" ", " mươi ", " trăm ", " ngàn ", " triệu ", " tỉ ", " ngàn tỉ ", " chục ngàn tỉ ", " trăm ngàn tỉ "]

function ParseNumbertoString(value) {
    var number = value.split(',');
    var str = "";
    for (let i = 0; i < number.length; i++) {
        var index = number.length - i > 1 ? number.length - i + 1 : 0;
        if (number[i] > 0)
            str += Format(number[i], i) + arrUnit[index]
    }

    return str.charAt(0).toUpperCase() + str.substring(1) + "đồng";
}

function Format(value, unit) {
    var str = "";
    var number = Number(value);
    var interger = 0;
    var surplus = 0;
    var isHundred = false;
    var isDozens = false;
    if (Math.floor(number / 100) > 0) {
        interger = Math.floor(number / 100)
        surplus = number % 100;
        str += arrNumber[interger] + arrUnit[2]
        number = number - interger * 100;
        isHundred = true;
    }
    else if (interger == 0 && unit > 0) {
        str += arrNumber[interger] + arrUnit[2]
    }
    if (number != 11) {
        if (Math.floor(number / 10) > 0) {
            interger = Math.floor(number / 10) > 1 ? Math.floor(number / 10) : 10
            surplus = number % 10;
            str += arrNumber[interger] + arrUnit[(interger == 10 ? 0 : 1)]
            number = number - Math.floor(number / 10) * 10;
            isDozens = true;
        }
        if (number > 0) {
            var strtmp = arrNumber[number];
            if (isHundred && !isDozens || interger == 0 && surplus == 0 && unit > 0)
                strtmp = " lẽ " + arrNumber[number];
            if (isHundred && isDozens || !isHundred && isDozens)
                strtmp = arrNumber[number];
            if (isHundred && isDozens && number == 1 || !isHundred && isDozens && number == 1)
                strtmp = " mốt ";
            if (isHundred && isDozens && number == 5 || !isHundred && isDozens && number == 5)
                strtmp = " lăm ";
            str += strtmp
        }
    }
    else {
        str += " mười một "
    }
    return str.trim();
}

$.BlockUI = function () {
    $(".content-wrapper").addClass("Block-UI").append('<div class="blockUI"><h1><i class="icon-spinner spinner" style="font-size:8rem; color: #ddd"></i></h1></div>')
}

$.unBlockUI = function () {
    $(".content-wrapper").removeClass("Block-UI").find(".blockUI").remove();
}

$(document).on("click", ".form-print", function () {
    var html = document.getElementById("print-form").innerHTML;
    var print = window.open('', '', 'left=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
    print.document.write(html);
    print.document.close();
    print.focus();
    print.print();
    print.close();
})

$.printUI = function (html) {
    var frame = document.createElement('iframe');
    frame.name = "framePrint";
    frame.style.position = "absolute";
    frame.style.top = "-1000000px";
    document.body.appendChild(frame);
    var frameDoc = frame.contentWindow ? frame.contentWindow : frame.contentDocument.document ? frame.contentDocument.document : frame.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write(html);
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["framePrint"].focus();
        window.frames["framePrint"].print();
        document.body.removeChild(frame);
    }, 500);
    return false;
}

$(document).on("click",".ads-title",function () {
    $(".ads-search-details").toggleClass("show")
});

$(document).on("click", "span.select-title", function () {
    var id = $(this).parents(".input-select").attr("id")
    $("#" + id + " .select-input-table").toggleClass("show");
    var span = $(this).offset();
    $("#" + id + " .select-input-table").offset({ top: Number(span.top) - Number($("#" + id + " .select-input-table").height()) - 7, left: Number(span.left) - 1 });
})

$(document).on("click", "li.select-input-item", function (e) {
    var id = $(this).parents(".input-select").attr("id")
    $("#" + id + " .select-input-span").removeClass("select-input-item-selected")
    $(this).find(".select-input-span-" + $(this).data("id")).addClass("select-input-item-selected")
    $("#" + id + " .select-input-table").toggleClass("show");
    $("#" + id + " .select-title").text($(this).find(".select-input-span-" + $(this).data("id")).text());
    $("#" + id + " .input-select").val($(this).data("id"));
    e.stopPropagation();
})

$(document).on("keyup", ".select-input-search input[type='search']", function () {
    var id = $(this).parents(".input-select").attr("id");
    var value = $(this).val().toLowerCase();
    $("#" + id + " .select-input-span").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        var span = $("#" + id + " span.select-title").offset();
        $("#" + id + " .select-input-table").offset({ top: Number(span.top) - Number($("#" + id + " .select-input-table").height()) - 7, left: Number(span.left) - 1 });
    })
})

$(document).on("click", ".printItem", function () {
    var div = document.getElementById("print");
    var lstCanvas = div.getElementsByTagName("canvas");
    for (let i = 0; i < lstCanvas.length; i++) {
        var img = document.createElement("img");
        img.src = lstCanvas[i].toDataURL();
        img.style.width = '450px';
        img.style.height = '300px';
        lstCanvas[i].parentElement.style.display = "none";
        lstCanvas[i].parentElement.parentElement.style.width = '450px';
        lstCanvas[i].parentElement.parentElement.style.height = '300px';
        lstCanvas[i].parentElement.parentElement.appendChild(img);
    }

    printHTML(div.innerHTML);
    for (let i = 0; i < lstCanvas.length; i++) {
        lstCanvas[i].parentElement.style.display = "block";
        lstCanvas[i].parentElement.parentElement.getElementsByTagName('img')[0].remove();
    }
    //printJS({ printable: 'print', type: 'html', style: '@page{size: A4; margin: 5mm 5mm 5mm 5mm;} body { font-weight: bold} ' });
});

printHTML = function (html) {
    var contents = html;
    var frame1 = document.createElement('iframe');
    frame1.name = "frame1";
    frame1.style.position = "absolute";
    frame1.style.top = "-1000000px";
    document.body.appendChild(frame1);
    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
    frameDoc.document.open();
    var linkcss = '<link href="/assets/css/print_custom.css" rel="stylesheet" media="print" /><link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css"><link href="/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css">'
    frameDoc.document.write('<html><head>' + linkcss + '</head><body>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        document.body.removeChild(frame1);
    }, 500);
    return false;
};

$(document).on("input", ".format-time", function () {

    var el = $(this);

    el.keyup(function (e) {
        if (e.keyCode == 8) {
            if (el.val().endsWith(":"))
                el.val(el.val().substring(0, 2))
            return;
        }
    })

    var maxLength = 5;
    var maxh = 23;
    var maxp = 59
    var number = /^\d+$/;
    if (number.test(el.val().replace(/[:]/g, ''))) {
        var time = el.val().split(":");

        if (el.val().length > maxLength)
            el.val(el.val().slice(0, -1));

        if (el.val().length == 2)
            el.val(el.val() + ":")

        if (el.val().charAt(2) !== "")
            if (el.val().charAt(2) !== ":") {
                el.val(el.val().substring(0, 2) + ":" + el.val().charAt(2))
                time[0] = el.val().substring(0, 2)
            }

        if (Number(time[0]) <= maxh)
            return;
        else
            el.val(el.val().slice(0, -1));



        if (Number(time[1]) <= maxp)
            return;
        else
            el.val(el.val().slice(0, -1));
    }
    else {
        el.val(el.val().slice(0, -1));
    }
});


var Select2MultiLv = function (model, select, selectId, filterObject) {
    $.ajax({
        url: "/SelectLevels/" + model,
        method: "GET",
        data: filterObject,
        dataType: "json",
        success: function (data) {
            $("#" + select).select2({
                minimumResultsForSearch: -1,
                placeholder: '-- Select --',
                data: data,
                formatSelection: function (item) {
                    return item.text
                },
                formatResult: function (item) {
                    return item.text
                },
                templateResult: formatResult,
            });
            if (selectId) {
                $("#" + select).val(selectId).trigger('change.select2');
            }
        },
        error: function (error) {
        }
    })
}

function formatResult(node) {
    var $result = $('<span style="padding-left:' + (20 * node.level) + 'px;">' + node.text + '</span>');
    return $result;
}

var Select2AddItem = function (url, select, filterObject) {
    $.ajax({
        url: url,
        method: "GET",
        data: filterObject,
        dataType: "json",
        success: function (data) {
            var places = [];
            $(data).each(function (index, value) {
                places.push(value);
            });
            var selectId = $("#" + select);
            if (selectId.prop) {
                var options = selectId.prop('options');
            }
            else {
                var options = selectId.attr('options');
            }
            $('option', selectId).remove();
            options[options.length] = new Option("-- Select --", "");
            $.each(places, function (index, item) {
                options[options.length] = new Option(item.text, item.id);
            });         
        },
        error: function (error) {
        }
    })
}

