<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeChatMenu.aspx.cs" Inherits="WeChatMenu.WeChatMenu" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head runat="server">
    <title>微信菜单管理</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=0" />
    <script src="js/jquery-2.0.0.min.js" type="text/javascript"></script>
</head>
<body style="background-color: #f8f8f8;">
    <form id="form1" runat="server">
    <div style="height:5px;"></div>
    <p style="margin:15px;"><strong>请输入公众号的Appid、AppSecret</strong></p>
    <div style="margin: 15px;padding:15px; background-color: #ededed;">
        <label>Appid:</label>
        <asp:TextBox ID="txt_appid" runat="server" placeholder="请输入AppId" autocomplete="off"
            MaxLength="18" spellcheck="false" style="color:#666666; width:175px; height:30px; font-size:16px;"></asp:TextBox>
        &nbsp;&nbsp;
        <label>AppSecret:</label>
        <asp:TextBox ID="txt_appsecret" runat="server" placeholder="请输入AppSecret" autocomplete="off"
            MaxLength="32" spellcheck="false" style="color:#666666; width:300px; height:30px; font-size:16px;"></asp:TextBox>
    </div>
    <div style="margin: 15px;padding:15px; background-color: #ededed;-moz-user-select:none;-webkit-user-select:none;user-select:none;">
        <asp:Button ID="btn_getmenu" runat="server" class="btn" Text="拉取菜单" OnClientClick="return checkget()"
            OnClick="btn_getmenu_Click" />&nbsp;
        <asp:Button ID="btn_updatemenu" runat="server" class="btn" Text="更新菜单" OnClientClick="return checkupdate()"
            OnClick="btn_updatemenu_Click" />&nbsp;
        <asp:Button ID="btn_backmenu" runat="server" class="btn" Text="导出菜单" OnClientClick="return checkupdate()"
            OnClick="btn_backmenu_Click" />&nbsp;
        <input type="button" class="btn" value="从文件更新" onclick="checkback()" /> &nbsp;
        <asp:Button ID="btn_delmenu" runat="server" class="btn" Text="删除菜单" OnClientClick="return checkdel()"
            style="background:#ff4c4c;" OnClick="btn_delmenu_Click" />&nbsp;&nbsp;&nbsp;
        <input id="chk_back" runat="server" type="checkbox" style="width:14px;" checked="checked" />
        <label for="chk_back" style="cursor: pointer;">更新成功后导出菜单</label>
    </div>
    <div style="margin:15px; background-color:White;width:1100px;">
        <table class="table" style="width: 1100px;">
            <thead>
                <tr>
                    <td class="td_shengru">
                        <strong>深度</strong>
                    </td>
                    <td class="td_shengru" style="text-align: center;">
                        第一列
                    </td>
                    <td class="td_shengru" style="text-align: center;">
                        第二列
                    </td>
                    <td class="td_shengru" style="text-align: center;">
                        第三列
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class="td_shengru">
                        主菜单按钮
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top1" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top1" runat="server" onchange="chengesel(this.id,1)">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key1" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top2" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top2" runat="server" onchange="chengesel(this.id,2)">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key2" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top3" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top3" runat="server" onchange="chengesel(this.id,3)">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key3" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td_shengru">
                        二级菜单No.1
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top1">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top11" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top11" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key11" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top2">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top21" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top21" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key21" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top3">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top31" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top31" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key31" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td_shengru">
                        二级菜单No.2
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top1">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top12" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top12" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key12" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top2">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top22" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top22" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key22" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top3">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top32" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top32" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key32" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td_shengru">
                        二级菜单No.3
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top1">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top13" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top13" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key13" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top2">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top23" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top23" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key23" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top3">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top33" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top33" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key33" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td_shengru">
                        二级菜单No.4
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top1">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top14" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top14" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key14" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top2">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top24" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top24" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key24" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top3">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top34" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top34" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key34" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="td_shengru">
                        二级菜单No.5
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top1">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top15" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top15" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key15" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top2">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top25" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top25" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key25" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="td_shengru">
                        <table border="0" class="innertable top3">
                            <tr>
                                <td class="td_titleName">
                                    名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Top35" runat="server" CssClass="txtNameValue"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    类型:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDL_Top35" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_titleName">
                                    key/url:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Key35" runat="server" CssClass="txtNameUrl" TextMode="MultiLine"
                                        Rows="1" onfocus="setStyle(this)" onblur="reducedStyle(this)" spellcheck="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="margin:15px;">
        <article>
            <h1>使用说明：</h1>
            <section>
                <h2 class="title">微信菜单更新</h2>
                <section>
                    <h3>1.1 使用前须知</h3>
                    <p>
                        　1、公众号必须已获得自定义菜单创建接口的权限<br>
                        　2、服务器不会缓存你任何数据，Appid、AppSecret仅在本地浏览器缓存<br>
                        　3、暂不支持含有小程序的菜单类型 miniprogram<br>
                        　4、暂不支持个性化菜单
                    </p>
                </section>
                <section>
                    <h3>1.2 请注意：</h3>
                    <p>
                        　1、自定义菜单最多包括3个一级菜单，每个一级菜单最多包含5个二级菜单。<br>
                        　2、一级菜单最多4个汉字，二级菜单最多7个汉字，多出来的部分将会以“...”代替。<br>
                        　3、创建自定义菜单后，菜单的刷新策略是，在用户进入公众号会话页或公众号profile页时，
                          如果发现上一次拉取菜单的请求在5分钟以前，就会拉取一下菜单，如果菜单有更新，就会刷新客户端的菜单。
                        测试时可以尝试取消关注公众账号后再次关注，则可以看到创建后的效果。
                    </p>
                </section>
                <section>
                    <h3>1.3 自定义菜单接口可实现多种类型按钮，如下：</h3>
                    <p>
                        　1、click：点击推事件用户点击click类型按钮后，微信服务器会通过消息接口推送消息类型为event的结构给开发者（参考消息接口指南），
                        并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互；<br>
                        　2、view：跳转URL用户点击view类型按钮后，微信客户端将会打开开发者在按钮中填写的网页URL，可与网页授权获取用户基本信息接口结合，获得用户基本信息。<br>
                        　3、scancode_push：扫码推事件用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后显示扫描结果（如果是URL，将进入URL），
                        且会将扫码的结果传给开发者，开发者可以下发消息。<br>
                        　4、scancode_waitmsg：扫码推事件且弹出“消息接收中”提示框用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后，
                        将扫码的结果传给开发者，同时收起扫一扫工具，然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。<br>
                        　5、pic_sysphoto：弹出系统拍照发图用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，
                        同时收起系统相机，随后可能会收到开发者下发的消息。<br>
                        　6、pic_photo_or_album：弹出拍照或者相册发图用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。
                        用户选择后即走其他两种流程。<br>
                        　7、pic_weixin：弹出微信相册发图器用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，将选择的相片发送给开发者的服务器，
                        并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。<br>
                        　8、location_select：弹出地理位置选择器用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，
                        同时收起位置选择工具，随后可能会收到开发者下发的消息。<br>
                        　9、media_id：下发消息（除文本消息）用户点击media_id类型按钮后，微信服务器会将开发者填写的永久素材id对应的素材下发给用户，永久素材类型可以是图片、
                        音频、视频、图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。<br>
                        　10、view_limited：跳转图文消息URL用户点击view_limited类型按钮后，微信客户端将打开开发者在按钮中填写的永久素材id对应的图文消息URL，
                        永久素材类型只支持图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。<br>
                    </p>
                </section>
                <section>
                    <h3 style="color:Red">1.4 更新须知</h3>
                    <p>
                        　1、当子菜单标题为空或子菜单事件值(如URL)为空时，该子菜单将不创建。<br>
                        　2、当一级菜单标题为空时，该一级菜单包括其下面的子菜单不创建。<br>
                        　3、当一级菜单绑定事件时，该一级菜单下面的子菜单将不创建，只有一级菜单未绑定事件时二级菜单才会生效<br>
                        　4、如出现错误请点击查看错误码进行对照或联系软件作者<br>
                        　5、网页链接不能超过256字节，如果URL过长将失效，URL必须以http://或https://开头<br>
                    </p>
                </section>
            </section>
            <secion>
                <h3>1.5 官方文档</h3>
                <p>　<a href="https://mp.weixin.qq.com/wiki?t=resource/res_main&amp;id=mp1421141013" target="_blank">微信公众平台菜单创建说明(官方)</a></p>
            </secion>
        </article>
    </div>
    <!--从json更新-->
    <div id="menubak" style="display:none;">
        <div class="mask"></div>
        <div class="dialog">
            <asp:TextBox ID="txt_bak" runat="server" CssClass="txtNameUrl" TextMode="MultiLine" style="width:100%;height:320px;overflow:auto;" 
                placeholder="请将备份的菜单数据粘贴到此处" spellcheck="false"></asp:TextBox>
            <p></p>
            <asp:Button ID="btn_import" runat="server" class="btn" Text="更新菜单" OnClick="btn_import_Click" OnClientClick="return checkbakupdate()" />&nbsp;&nbsp;
            <input type="button" class="btn" value="关闭" style="background:#ff4c4c;" onclick="$('#menubak').fadeOut(200);" />
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var appid = localStorage.getItem('appid');
            var appsecret = localStorage.getItem('appsecret');
            if (appid != null && appsecret != null) {
                $("#txt_appid").val(appid);
                $("#txt_appsecret").val(appsecret);
            }
            chengesel('DDL_Top1', 1); chengesel('DDL_Top2', 2); chengesel('DDL_Top3', 3);
        })
        function checkget() {
            var appid = $("#txt_appid").val();
            var appsecret = $("#txt_appsecret").val();
            if (appid == "") {
                dialogMsg('请填appid');
                return false;
            }
            if (appsecret == "") {
                dialogMsg('请填appsecret');
                return false;
            }
            localStorage.setItem('appid', appid);
            localStorage.setItem('appsecret', appsecret);
            return true;
        }
        function checkupdate() {
            var appid = $("#txt_appid").val();
            var appsecret = $("#txt_appsecret").val();
            if (appid == "") {
                dialogMsg('请填appid');
                return false;
            }
            if (appsecret == "") {
                dialogMsg('请填appsecret');
                return false;
            }
            localStorage.setItem('appid', appid);
            localStorage.setItem('appsecret', appsecret);
            return true;
        }
        function checkback() {
            var appid = $("#txt_appid").val();
            var appsecret = $("#txt_appsecret").val();
            if (appid == "") {
                dialogMsg('请填appid');
                return false;
            }
            if (appsecret == "") {
                dialogMsg('请填appsecret');
                return false;
            }
            $('#menubak').fadeIn(200);
        }
        function checkbakupdate() {
            if (checkupdate()) {
                if ($('#txt_bak').val() == "") {
                    dialogMsg('请填备份的菜单数据');
                    return false;
                }
                return true;
            }
            return false;
        }
        function checkdel() {
            var appid = $("#txt_appid").val();
            var appsecret = $("#txt_appsecret").val();
            if (appid == "") {
                dialogMsg('请填appid');
                return false;
            }
            if (appsecret == "") {
                dialogMsg('请填appsecret');
                return false;
            }
            return confirm("你确认删除菜单吗?\n删除前建议先备份当前菜单.");
        }
        function setStyle(x) {
            x.rows = "5";
            x.style.overflowY = "scroll";
        }
        function reducedStyle(x) {
            x.rows = "1";
            x.style.overflow = "hidden";
        }
        function chengesel(sel, num) {
            var value = $("#" + sel).val();
            if (value == "") {
                $(".top" + num).fadeIn(200);
                $("#txt_Key" + num).attr("disabled", "disabled")
            }
            else {
                $(".top" + num).fadeOut(200);
                $("#txt_Key" + num).removeAttr("disabled");
            }
        }
        function dialogMsg(msg, time, type) {
            if (time == undefined) {
                time = 4000;
            }
            if (type == undefined) {
                type = 'success';
            }
            MsgTips(time, msg, 300, type);
        }
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
    </script>
    <style type="text/css">
        *, *:before, *:after {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
        }
        *, *:before, *:after {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
        }
        body {
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-size: 14px;
            line-height: 1.428571429;
            color: #333333;
            background-color: #ffffff;
        }
        input,textarea,select {
            padding: 1px 2px;
            padding-left: 3px;
            line-height: 20px;
            border: 1px solid #d4d4d4;
            /*background: #fff;*/
            vertical-align: middle;
            color: #333;
            font-size: 100%;
            margin: 2px 0px 2px;
            width: 230px;
        }
        select{
        }
        .txtNameValue{
        }
        .txtNameUrl{
            overflow: hidden;
        }
        h1{
            background-color: #fff0de;
        }
        h3{
            background-color: #f9f2e8;
        }
        .btn{
            background: #16a0d3;
            border: none;
            color: #fff;
            cursor: pointer;
            display: inline-block;
            font-family: "Microsoft Yahei";
            font-size: 12px;
            height: 32px;
            line-height: 32px;
            margin: 0 1px 0 0;
            padding: 0 20px;
            width:100px;
        }
        .btn:hover{
            color: #000;
            background: #43ccff;
        }
        /*table*/
        .table {
        }
        .table tr td {
        }
        .table tr td:hover {
          background-color:#dddddd;
        }
        table {
            max-width: 100%;
            background-color: transparent;
        }
        table {
            border-collapse: collapse;
            border-spacing: 0;
        }
        .innertable{
            border: 0px solid #ffffff;
        }
        .td_shengru{
            border: 1px solid #dddddd;
            padding:10px;
        }
        /*头部提示*/
        .mtip>span {
	        vertical-align:3px;
	        line-height:1;
	        display:inline-block;
	        width:auto;
            font-size:24px;
        }
        .mtip {
	        border-radius:0 0 4px 4px;
            padding-top:10px;
            padding-left:25px;
            padding-right:25px;
            padding-bottom:10px;
            color:#fff;
	        text-shadow:0 1px 0 rgba(0,0,0,0.2);
	        box-shadow:0 4px 4px rgba(0,0,0,0.2)
        }
        .mtip.error {
	        background-color:#BF3358;
	        background-image:-moz-linear-gradient(top,#e34447,#BF3358);
	        background-image:-ms-linear-gradient(top,#e34447,#BF3358);
	        background-image:-webkit-gradient(linear,0 0,0 100%,from(#e34447),to(#BF3358));
	        background-image:-webkit-linear-gradient(top,#e34447,#BF3358);
	        background-image:-o-linear-gradient(top,#e34447,#BF3358);
	        background-image:linear-gradient(top,#e34447,#BF3358);
	        border:1px solid #ca3e3e
        }
        .mtip.success {
	        background-color:#43ab00;
	        background-image:-moz-linear-gradient(top,#43ab00,#388e00);
	        background-image:-ms-linear-gradient(top,#43ab00,#388e00);
	        background-image:-webkit-gradient(linear,0 0,0 100%,from(#43ab00),to(#388e00));
	        background-image:-webkit-linear-gradient(top,#43ab00,#388e00);
	        background-image:-o-linear-gradient(top,#43ab00,#388e00);
	        background-image:linear-gradient(top,#43ab00,#388e00);
	        border:1px solid #338100
        }
        .mtip.warning {
	        background-color:  orange;
	        background-image:-moz-linear-gradient(top,#0f76cd,#086cc1);
	        background-image:-ms-linear-gradient(top,#0f76cd,#086cc1);
	        background-image:-webkit-gradient(linear,0 0,0 100%,from(#0f76cd),to(#086cc1));
	        background-image:-webkit-linear-gradient(top,#0f76cd,#086cc1);
	        background-image:-o-linear-gradient(top,#0f76cd,#086cc1);
	        background-image:linear-gradient(top,#0f76cd,#086cc1);
	        border:1px solid #006096
        }
        .tip_container {
	        display:none;
            z-index: 9999;
            position: fixed;
            top: 0;
            text-align: left;
            width: auto;
            _width: auto;
            left:40%;
        }
        .mask {
          position: fixed;
          z-index: 1000;
          top: 0;
          right: 0;
          left: 0;
          bottom: 0;
          background: rgba(0, 0, 0, 0.6);
        }
        .dialog {
            position:fixed;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            padding: 8px;
            z-index: 1002;
            top: 15%;
            left: 12%;
            right: 12%;
            height:420px;
            padding:20px;
            background:#fff;
            overflow:auto;
        }
    </style>
    </form>
</body>
</html>