using Newtonsoft.Json.Linq;

namespace WeChatMenu
{
    /// <summary>
    /// 微信公众号菜单设置
    /// </summary>
    public class WeChatMenu1 : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(System.Web.HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request["action"];
            string appid = context.Request["appid"];
            string appsecret = context.Request["appsecret"];
            string json = context.Request["json"];

            string ls_response = "";
            switch (action)
            {
                case "GETMENU"://获取现有菜单
                    ls_response = GetMenu(appid, appsecret);
                    break;
                case "UPDATEMENU"://更新菜单
                    ls_response = UpdateMenu(appid, appsecret, json);
                    break;
                case "DELETEMENU"://删除菜单
                    ls_response = DelMenu(appid, appsecret);
                    break;
                default:
                    ls_response = "{\"errcode\":-1,\"errmsg\":\"无效的命令\" }";
                    break;
            }
            context.Response.Write(ls_response);
            context.Response.End();
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        private string GetMenu(string appid, string appsecret)
        {
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(appsecret))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不能为空\" }";
            string access_token = "";
            if (!Get_Access_token(appid, appsecret, out access_token))
                return access_token;
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token);
            string ls_rc = HttpGet(url);//获取当前菜单
            JObject json = JObject.Parse(ls_rc);
            return json.ToString();
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        private string UpdateMenu(string appid, string appsecret, string json)
        {
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(appsecret))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不能为空\" }";
            string access_token = "";
            if (!Get_Access_token(appid, appsecret, out access_token))
                return access_token;
            if (string.IsNullOrEmpty(json))
                return "{\"errcode\":-2,\"errmsg\":\"json不能为空\" }";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);
            try
            {
                string ls_json = json;
                JObject _json = JObject.Parse(ls_json);
                json = _json.ToString();
            }
            catch (System.Exception ex) { return "{\"errcode\":-2,\"errmsg\":\"" + ex.Message + "\" }"; }
            string ls_rc = HttpPost(url, json);
            if (ls_rc.Substring(0, 2) == "-1")
                return "{\"errcode\":-2,\"errmsg\":\"菜单更新失败<br>" + HTTPERROR + "\" }";
            JObject jo = JObject.Parse(ls_rc);
            if (jo["errmsg"].ToString() == "ok")
                return "{\"errcode\":0,\"errmsg\":\"菜单更新成功\" }";
            else
            {
                if (jo["errcode"].ToString() == "40013")
                    jo["errmsg"] = "不合法的access_token，请开发者认真比对access_token的有效性（如是否过期），或查看是否正在为恰当的公众号调用接口";
                return "{\"errcode\":-2,\"errmsg\":\"菜单更新失败<br>" + jo["errmsg"].ToString() + "\" }";
            }
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        private string DelMenu(string appid, string appsecret)
        {
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(appsecret))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不能为空\" }";
            string access_token = "";
            if (!Get_Access_token(appid, appsecret, out access_token))
                return access_token;
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_token);
            string ls_Message = HttpGet(url);//获取当前菜单
            JObject json = JObject.Parse(ls_Message);
            if (json["errmsg"].ToString() == "ok")
                return "{\"errcode\":-2,\"errmsg\":\"公众号菜单已成功删除\" }";
            else
                return "{\"errcode\":-2,\"errmsg\":\"菜单删除失败:<br>" + json["errmsg"].ToString() + "\" }";

        }
        /// <summary>
        /// 执行HTTP访问返回错误信息
        /// </summary>
        string HTTPERROR = string.Empty;
        /// <summary>
        /// 获取access_token
        /// <para>这里不校验是否正确</para>
        /// <para>如果Session有效期间在其他地方更新了access_token,请更换浏览器</para>
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="secret">appsecret</param>
        private bool Get_Access_token(string appid, string appsecret, out string access_token)
        {
            if (System.Web.HttpContext.Current.Session["appid"] != null)
            { access_token = System.Web.HttpContext.Current.Session["appid"].ToString(); return true; }
            string ls_appid = appid.Replace(" ", "");
            string ls_secret = appsecret.Replace(" ", "");
            access_token = "";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", ls_appid, ls_secret);
            string ls_rc = HttpGet(url);
            if (ls_rc.Substring(0, 2) == "-1")
            { access_token = "{\"errcode\":-2,\"errmsg\":\"无法连接到远程服务器，请检查网络设置\" }"; return false; }
            JObject ja = JObject.Parse(ls_rc);
            try { access_token = ja["access_token"].ToString(); }
            catch
            {
                if (ja["errcode"].ToString() == "-1")
                    ja["errmsg"] = "系统繁忙，请稍候再试";
                if (ja["errcode"].ToString() == "40001")
                    ja["errmsg"] = "AppSecret错误或者AppSecret不属于这个公众号，请确认AppSecret的正确性";
                if (ja["errcode"].ToString() == "40164")
                    ja["errmsg"] = "调用接口的IP地址不在白名单中，请在接口IP白名单中进行设置";
                if (ja["errcode"].ToString() == "40013")
                    ja["errmsg"] = "无效的Appid";
                access_token = ja.ToString(); return false;
            }
            System.Web.HttpContext.Current.Session["appid"] = access_token;
            return true;
        }
        /// <summary>
        /// Get方法获取网页信息
        /// </summary>
        public string HttpGet(string as_url)
        {
            HTTPERROR = string.Empty;
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Accept-Language", "zh-cn");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("User-Agent", "Baiduspider+(+http://www.baidu.com/search/spider.htm)");
            webClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            try
            {
                return webClient.DownloadString(as_url);
            }
            catch (System.Net.WebException ex)
            {
                HTTPERROR = ex.ToString();
                return "-1," + ex.ToString();
            }
            catch (System.Exception ex)
            {
                HTTPERROR = ex.ToString();
                return "-1," + ex.ToString();
            }
            finally
            {
                if (webClient != null) webClient.Dispose();
                if (webClient != null) webClient = null;
            }
        }
        /// <summary>
        /// HttpPost访问
        /// </summary>
        public string HttpPost(string Url, string Params)
        {
            HTTPERROR = string.Empty;
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Accept-Language", "zh-cn");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("User-Agent", "Baiduspider+(+http://www.baidu.com/search/spider.htm)");
            //将字符串转换成字节数组
            byte[] postData = System.Text.Encoding.GetEncoding("utf-8").GetBytes(Params);
            try
            {
                byte[] responseData = webClient.UploadData(Url, "POST", postData);
                return System.Text.Encoding.GetEncoding("utf-8").GetString(responseData);
            }
            catch (System.Exception Exce)
            {
                HTTPERROR = Exce.ToString();
                return "-1," + Exce.ToString();
            }
            finally
            {
                if (webClient != null) webClient.Dispose();
                if (webClient != null) webClient = null;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}