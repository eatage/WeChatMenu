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
                case "GETMENU":
                    ls_response = GetMenu(appid, appsecret);
                    break;
                case "UPDATEMENU":
                    ls_response = UpdateMenu(appid, appsecret, json);
                    break;
                case "BAKMENU":
                    ls_response = BakMenu(json);
                    break;
                case "DELETEMENU":
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
            string access_token = Get_Access_token(appid, appsecret);
            if (string.IsNullOrEmpty(access_token))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不对，请检查后再操作\" }";
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
            string access_token = Get_Access_token(appid, appsecret);
            if (string.IsNullOrEmpty(access_token))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不对，请检查后再操作\" }";
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
            JObject jo = JObject.Parse(ls_rc);
            if (jo["errmsg"].ToString() == "ok")
                return "{\"errcode\":0,\"errmsg\":\"菜单更新成功\" }";
            else
                return "{\"errcode\":-2,\"errmsg\":\"菜单更新失败<br>" + jo["errmsg"].ToString() + "\" }";
        }
        /// <summary>
        /// 备份菜单
        /// <para>此方法是为了格式化菜单</para>
        /// </summary>
        private string BakMenu(string json)
        {
            if (string.IsNullOrEmpty(json))
                return "{\"errcode\":-2,\"errmsg\":\"json不能为空\" }";
            try
            {
                string ls_json = json;
                JObject _json = JObject.Parse(ls_json);
                json = _json.ToString();
            }
            catch (System.Exception ex) { return "{\"errcode\":-2,\"errmsg\":\"" + ex.Message + "\" }"; }
            return json;
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        private string DelMenu(string appid, string appsecret)
        {
            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(appsecret))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不能为空\" }";
            string access_token = Get_Access_token(appid, appsecret);
            if (string.IsNullOrEmpty(access_token))
                return "{\"errcode\":-2,\"errmsg\":\"Appid或AppSecret不对，请检查后再操作\" }";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_token);
            string ls_Message = HttpGet(url);//获取当前菜单
            JObject json = JObject.Parse(ls_Message);
            if (json["errmsg"].ToString() == "ok")
                return "{\"errcode\":-2,\"errmsg\":\"公众号菜单已成功删除\" }";
            else
                return "{\"errcode\":-2,\"errmsg\":\"菜单删除失败:<br>" + json["errmsg"].ToString() + "\" }";

        }
        /// <summary>
        /// 获取access_token
        /// <para>这里不校验是否正确</para>
        /// <para>如果Session有效期间在其他地方更新了access_token,请更换浏览器</para>
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="secret">appsecret</param>
        private string Get_Access_token(string appid, string appsecret)
        {
            if (System.Web.HttpContext.Current.Session["appid"] != null) return System.Web.HttpContext.Current.Session["appid"].ToString();
            string ls_appid = appid.Replace(" ", "");
            string ls_secret = appsecret.Replace(" ", "");
            string access_token = "";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", ls_appid, ls_secret);
            string ls_rc = HttpGet(url);
            JObject ja = JObject.Parse(ls_rc);
            try { access_token = ja["access_token"].ToString(); }
            catch { return ""; }
            System.Web.HttpContext.Current.Session["appid"] = access_token;
            return access_token;
        }
        /// <summary>
        /// Get方法获取网页信息
        /// </summary>
        public static string HttpGet(string as_url)
        {
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
                return "-1," + ex.ToString();
            }
            catch (System.Exception ex)
            {
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