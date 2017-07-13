$(function () {
    //从缓存写入数据
    var appid = localStorage.getItem('appid');
    var appsecret = localStorage.getItem('appsecret');
    if (appid !== null && appsecret !== null) {
        $("#txt_appid").val(appid);
        $("#txt_appsecret").val(appsecret);
    }
    //绘制table
    var tablehtml = "";
    tablehtml += '<table class="table" style="width: 1100px;">';
    tablehtml += '<thead><tr><td class="td_shengru"><strong>深度</strong></td><td class="td_shengru" style="text-align: center;">' +
                '第一列</td><td class="td_shengru" style="text-align: center;">第二列</td>' +
                '<td class="td_shengru" style="text-align: center;">第三列</td></tr></thead>';
    tablehtml += '<tbody>';
    for (var i = 0; i < 6; i++) {
        if (i === 0) tablehtml += '<tr><td class="td_shengru">主菜单按钮</td>';
        else tablehtml += '<tr><td class="td_shengru">二级菜单No.' + i + '</td>';
        for (var j = 0; j < 3; j++) {
            var id = (j + 1) + '';
            if (i > 0) id += i;
            var tableclass = "";
            if (i > 0) tableclass += ' top' + (j + 1);
            tablehtml += '<td class="td_shengru"><table border="0" class="innertable' + tableclass + '">' +
                '<tr><td>名称:</td><td>' +
                '<input id="txt_Top' + id + '" type="text" class="txtNameValue" /></td></tr>' +
                '<tr><td>类型:</td><td>' +
                '<select id="DDL_Top' + id + '" onchange="chengesel(this.id,' + (j + 1) + ')"></select></td></tr>' +
                '<tr><td>key/url:</td><td>' +
                '<textarea id="txt_Key' + id + '" class="txtNameUrl" Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></textarea>' +
                '</td></tr></table></td>';
        }
        tablehtml += '</tr>';
    }
    tablehtml += '</tbody>';
    tablehtml += '</table>';
    document.getElementById('menutable').innerHTML = tablehtml;
    //为所有select添加Option
    for (var k = 0; k < 3; k++) {
        Typemenu("DDL_Top" + (k + 1));
        for (var m = 0; m < 5; m++) {
            Typemenu("DDL_Top" + (k + 1) + (m + 1));
        }
    }
    chengesel('DDL_Top1', 1); chengesel('DDL_Top2', 2); chengesel('DDL_Top3', 3);
})
$(document).ready(function () {
    $("button").click(function () {
        switch (this.id) {
            case "getmenu":
                getmenu();
                break;
            case "updatemenu":
                updatemenu();
                break;
            case "bakmenu":
                bakmenu();
                break;
            case "checkback":
                checkback();
                break;
            case "delmenu":
                delmenu();
                break;
            case "btn_import":
                importmenu();
                break;
        }
    });
});
//为select控件赋值
function Typemenu(id) {
    $("#" + id).append("<option value=''></option>");
    $("#" + id).append("<option value='click'>点击事件</option>");
    $("#" + id).append("<option value='view'>跳转URL</option>");
    $("#" + id).append("<option value='scancode_push'>扫码事件</option>");
    $("#" + id).append("<option value='scancode_waitmsg'>扫码推事件且弹出“消息接收中”提示框</option>");
    $("#" + id).append("<option value='pic_sysphoto'>弹出系统拍照发图</option>");
    $("#" + id).append("<option value='pic_photo_or_album'>弹出拍照或者相册发图</option>");
    $("#" + id).append("<option value='pic_weixin'>弹出微信相册发图器</option>");
    $("#" + id).append("<option value='location_select'>弹出地理位置选择器</option>");
    $("#" + id).append("<option value='media_id'>下发消息（除文本消息）</option>");
    $("#" + id).append("<option value='view_limited'>跳转图文消息URL</option>");
}
//校验AppidAndAppsecret
function checkAppidAndAppsecret() {
    var appid = $("#txt_appid").val();
    var appsecret = $("#txt_appsecret").val();
    if (appid === "") {
        dialogMsg('请填appid');
        return false;
    }
    if (appsecret === "") {
        dialogMsg('请填appsecret');
        return false;
    }
    localStorage.setItem('appid', appid);
    localStorage.setItem('appsecret', appsecret);
    return true;
}
//打开从备份数据更新窗口
function checkback() {
    if (checkAppidAndAppsecret()) {
        $("#txt_bak").val('');
        $("#btn_import").show();
        $('#menubak').fadeIn(200);
    }
}
//从备份数据更新
function importmenu() {
    var menu = $("#txt_bak").val();
    if (menu === "") {
        dialogMsg('请填入备份的菜单数据');
        return;
    }
    updatemenu(menu);
}
//从服务器拉取菜单并赋值
function getmenu() {
    if (checkAppidAndAppsecret()) {
        $.ajax({
            type: 'post',
            dataType: "text",
            url: "WeChatMenu.ashx",
            data: { action: 'GETMENU', appid: $("#txt_appid").val(), appsecret: $("#txt_appsecret").val() },
            async: false,
            success: function (data) {
                var json = eval("(" + data + ")");
                if (json.errmsg !== undefined) {
                    dialogMsg(json.errmsg, 4000, 'error');
                    return;
                }
                if (json.menu.button[0] !== undefined) {
                    for (var i = 0; i < 3; i++) {
                        var type = json.menu.button[i].type;
                        var name = json.menu.button[i].name;
                        var keyOrUrl = '';
                        if (json.menu.button[i].key === undefined) keyOrUrl = json.menu.button[i].url;
                        else keyOrUrl = json.menu.button[i].key;
                        $("#txt_Top" + (i + 1)).val(name);
                        $("#DDL_Top" + (i + 1)).val(type);
                        $("#txt_Key" + (i + 1)).val(keyOrUrl);
                        //没有子菜单
                        if (json.menu.button[i].sub_button.length === 0) {
                            continue;
                        } else {
                            //有子菜单
                            var num = json.menu.button[i].sub_button.length
                            for (var j = 0; j < num; j++) {
                                var _type = json.menu.button[i].sub_button[j].type;
                                var _name = json.menu.button[i].sub_button[j].name;
                                var _keyOrUrl = '';
                                if (json.menu.button[i].sub_button[j].key === undefined) keyOrUrl = json.menu.button[i].sub_button[j].url;
                                else keyOrUrl = json.menu.button[i].sub_button[j].key;
                                $("#txt_Top" + (i + 1) + (j + 1)).val(_name);
                                $("#DDL_Top" + (i + 1) + (j + 1)).val(_type);
                                $("#txt_Key" + (i + 1) + (j + 1)).val(keyOrUrl);
                            }
                        }
                    }
                }
                chengesel('DDL_Top1', 1); chengesel('DDL_Top2', 2); chengesel('DDL_Top3', 3);
                dialogMsg('成功获取');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                dialogMsg(XMLHttpRequest.status + '<br>' + XMLHttpRequest.readyState + '<br>' + textStatus, 4000, 'error');
            }
        });
    }
}
//从控件中获取json
function getJson() {
    var json = '{"button": [';
    for (var i = 0; i < 3; i++) {
        var name = $("#txt_Top" + (i + 1)).val();
        var type = $("#DDL_Top" + (i + 1)).val();
        var key = $("#txt_Key" + (i + 1)).val();
        if (name === "" && i === 0) {
            dialogMsg('第一个一级菜单名称不允许为空', 4000, 'error');
            return "";
        }
        else if (name === "") {
            continue;
        }
        json += '{"name": "' + name + '",';
        if (type !== "") {
            if (key === "") {
                dialogMsg('第' + (i + 1) + '个一级菜单设置错误<br>已经选择了类型的情况下,key/url不允许为空', 4000, 'error');
                return "";
            }
            if (name === "") {
                json = json.substr(0, json.length - 1);
                continue;
            }
            json += '"type": "' + type + '",';
            if (type === "view")
                json += '"url": "' + key + '"},';
            else
                json += '"key": "' + key + '"},';
        }
        else {
            json += '"sub_button": [';
            for (var j = 0; j < 5; j++) {
                var _name = $("#txt_Top" + (i + 1) + (j + 1)).val();
                var _type = $("#DDL_Top" + (i + 1) + (j + 1)).val();
                var _key = $("#txt_Key" + (i + 1) + (j + 1)).val();
                if ((_name === "" || _type === "" || _key === "") && j === 0 && name !== "") {
                    dialogMsg('第' + (i + 1) + '个一级菜单下的第' + (j + 1) + '个二级菜单设置错误<br>对应一级菜单没有选择类型的情况下,第一个二级菜单必须有效', 4000, 'error');
                    return "";
                }
                if (_name === "" || _type === "" || _key === "") {
                    json = json.substr(0, json.length - 1);
                    break;
                }
                json += '{"type": "' + _type + '",';
                json += '"name": "' + _name + '",';
                if (_type === "view")
                    json += '"url": "' + _key + '"},';
                else
                    json += '"key": "' + _key + '"},';

            }
            json += ']},';
        }
    }
    json = json.substr(0, json.length - 1);
    json += ']}';
    return json;
}
//更新菜单
function updatemenu(menu) {
    if (checkAppidAndAppsecret()) {
        var _json = "";
        if (menu === undefined) _json = getJson();
        else _json = menu;
        if (_json !== "") {
            $.ajax({
                type: 'post',
                dataType: "text",
                url: "WeChatMenu.ashx",
                data: { action: 'UPDATEMENU', appid: $("#txt_appid").val(), appsecret: $("#txt_appsecret").val(), json: _json },
                async: false,
                success: function (data) {
                    var json = eval("(" + data + ")");
                    dialogMsg(json.errmsg);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    dialogMsg(XMLHttpRequest.status + '<br>' + XMLHttpRequest.readyState + '<br>' + textStatus, 4000, 'error');
                }
            });
        }
    }
}
//备份菜单
function bakmenu() {
    if (checkAppidAndAppsecret()) {
        var _json = getJson();
        if (_json !== "") {
            //格式化json
            _json = formatJson(_json);
            //csv格式需要在文件内容前加BOM头\ufeff 否则Excel打开将乱码
            var blob = new Blob([_json], { type: 'text/text,charset=UTF-8' });
            openDownloadDialog(blob, (new Date()).Format("yyyyMMddhhmmss") + '导出菜单.txt');
        }
    }
}
//删除前提示
function checkdel() {
    if (checkAppidAndAppsecret()) {
        return confirm("你确认删除菜单吗?\n删除前建议先备份当前菜单.");
    }
}
//删除菜单
function delmenu() {
    if (checkdel()) {
        $.ajax({
            type: 'post',
            dataType: "text",
            url: "WeChatMenu.ashx",
            data: { action: 'DELETEMENU', appid: $("#txt_appid").val(), appsecret: $("#txt_appsecret").val() },
            async: false,
            success: function (data) {
                var json = eval("(" + data + ")");
                dialogMsg(json.errmsg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                dialogMsg(XMLHttpRequest.status + '<br>' + XMLHttpRequest.readyState + '<br>' + textStatus, 4000, 'error');
            }
        });
    }
}
//key/url的textarea在获得焦点时放大
function setStyle(x) {
    x.rows = "5";
    x.style.overflowY = "scroll";
}
//key/url的textarea在失去焦点时还原
function reducedStyle(x) {
    x.rows = "1";
    x.style.overflow = "hidden";
}
//当一级菜单 绑定了事件时 隐藏对应的二级菜单
function chengesel(sel, num) {
    if (sel !== "DDL_Top1" && sel !== "DDL_Top2" && sel !== "DDL_Top3") {
        return;
    }
    var value = $("#" + sel).val();
    if (value === "") {
        $(".top" + num).fadeIn(200);
        $("#txt_Key" + num).attr("disabled", "disabled")
    }
    else {
        $(".top" + num).fadeOut(200);
        $("#txt_Key" + num).removeAttr("disabled");
    }
}
//提示信息
function dialogMsg(msg, time, type) {
    if (time === undefined) {
        time = 4000;
    }
    if (type === undefined) {
        type = 'success';
    }
    MsgTips(time, msg, 300, type);
}
//页面顶部居中位置的提示信息
function MsgTips(timeOut, msg, speed, type) {
    $(".tip_container").remove();
    var bid = parseInt(Math.random() * 100000);
    $("body").prepend('<div id="tip_container' + bid + '" class="container tip_container"><div id="tip' + bid + '" class="mtip"><span id="tsc' + bid + '"></span></div></div>');
    var $this = $(this);
    var $tip_container = $("#tip_container" + bid);
    var $tip = $("#tip" + bid);
    var $tipSpan = $("#tsc" + bid);
    //先清楚定时器
    clearTimeout(window.timer);
    //主体元素绑定事件
    $tip.attr("class", type).addClass("mtip");
    $tipSpan.html(msg);
    $tip_container.slideDown(speed);
    //提示层隐藏定时器
    window.timer = setTimeout(function () {
        $tip_container.slideUp(speed);
        $(".tip_container").remove();
    }, timeOut);
    $("#tip_container" + bid).css("left", ($(window).width() - $("#tip_container" + bid).width()) / 2);
}
/**
*通用的打开下载对话框方法，没有测试过具体兼容性
*@method 下载
*@param {string}url 下载地址，也可以是一个blob对象，必选
*@param {string}saveName 保存文件名，可选
 */
function openDownloadDialog(url, saveName) {
    if (typeof url === 'object' && url instanceof Blob) {
        url = URL.createObjectURL(url); // 创建blob地址
    }
    var aLink = document.createElement('a');
    aLink.href = url;
    aLink.download = saveName || ''; // HTML5新增的属性，指定保存文件名，可以不要后缀，注意，file:///模式下不会生效
    var event;
    if (window.MouseEvent) event = new MouseEvent('click');
    else {
        event = document.createEvent('MouseEvents');
        event.initMouseEvent('click', true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
    }
    aLink.dispatchEvent(event);
}
/**
 * 对Date的扩展，将 Date 转化为指定格式的String
 * 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
 * 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
 *@method 格式化日期
 *@param {Date}fmt 日期
 *@param {object}function 回调
 *@return {string}格式化的日期
 */
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}