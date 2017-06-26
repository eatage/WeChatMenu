using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;

namespace WeChatMenu
{
    public partial class WeChatMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMenuType();
            }
        }
        /// <summary>
        /// 绑定菜单类型控件
        /// </summary>
        private void BindMenuType()
        {
            for (int i = 0; i < 3; i++)
            {
                TypeMenu(GetDropDownListField("DDL_Top" + (i + 1)));
                for (int j = 0; j < 5; j++)
                    TypeMenu(GetDropDownListField("DDL_Top" + (i + 1) + (j + 1)));
            }
        }
        /// <summary>
        /// 绑定菜单事件类型
        /// </summary>
        public static void TypeMenu(DropDownList DDL)
        {
            DDL.Items.Add(new ListItem("", ""));
            DDL.Items.Add(new ListItem("点击事件", "click"));
            DDL.Items.Add(new ListItem("跳转URL", "view"));
            DDL.Items.Add(new ListItem("扫码事件", "scancode_push"));
            DDL.Items.Add(new ListItem("扫码推事件且弹出“消息接收中”提示框", "scancode_waitmsg"));
            DDL.Items.Add(new ListItem("弹出系统拍照发图", "pic_sysphoto"));
            DDL.Items.Add(new ListItem("弹出微信相册发图器", "pic_weixin"));
            DDL.Items.Add(new ListItem("弹出地理位置选择器", "location_select"));
            DDL.Items.Add(new ListItem("下发消息（除文本消息）", "media_id"));
            DDL.Items.Add(new ListItem("跳转图文消息URL", "view_limited"));
        }
        /// <summary>
        /// 从服务器拉取菜单
        /// </summary>
        protected void btn_getmenu_Click(object sender, EventArgs e)
        {
            string access_token = Get_Access_token(txt_appid.Text, txt_appsecret.Text);
            if (access_token == "")
            {
                RegisterScript("dialogMsg('Appid或AppSecret不对，请检查后再操作');");
                return;
            }
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token);
            string ls_Message = HttpGet(url);//获取当前菜单
            JObject json = JObject.Parse(ls_Message);
            #region 拉取菜单信息并为控件赋值
            for (int i = 0; i < 3; i++)
            {
                string menu1 = (i + 1).ToString();
                try { GetTextBoxField("txt_Top" + menu1).Text = json["menu"]["button"][i]["name"].ToString(); }
                catch
                { if (i == 0) { RegisterScript("dialogMsg('该微信号未上传菜单或菜单已删除，请重新设置全部菜单');"); return; } }
                try
                {
                    for (int j = 0; j < 5; j++)
                    {
                        string menu2 = (j + 1).ToString();
                        GetDropDownListField("DDL_Top" + menu1 + menu2).SelectedValue = json["menu"]["button"][i]["sub_button"][j]["type"].ToString();
                        GetTextBoxField("txt_Top" + menu1 + menu2).Text = json["menu"]["button"][i]["sub_button"][j]["name"].ToString();
                        try { GetTextBoxField("txt_Key" + menu1 + menu2).Text = json["menu"]["button"][i]["sub_button"][j]["url"].ToString(); }
                        catch { GetTextBoxField("txt_Key" + menu1 + menu2).Text = json["menu"]["button"][i]["sub_button"][j]["key"].ToString(); }
                    }
                }
                catch
                {
                    try
                    {
                        GetDropDownListField("DDL_Top" + menu1).SelectedValue = json["menu"]["button"][i]["type"].ToString();
                        GetTextBoxField("txt_Top" + menu1).Text = json["menu"]["button"][i]["name"].ToString();
                        try { GetTextBoxField("txt_Key" + menu1).Text = json["menu"]["button"][i]["url"].ToString(); }
                        catch { GetTextBoxField("txt_Key" + menu1).Text = json["menu"]["button"][i]["key"].ToString(); }
                    }
                    catch { }
                }
            }
            #endregion
            //RegisterScript("chengesel('DDL_Top1',1);chengesel('DDL_Top2',2);chengesel('DDL_Top3',3);localStorage.setItem('getmenu" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', '" + ls_Message + "');");
            RegisterScript("chengesel('DDL_Top1',1);chengesel('DDL_Top2',2);chengesel('DDL_Top3',3);");
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        protected void btn_updatemenu_Click(object sender, EventArgs e)
        {
            string menu = "";
            try
            {
                string ls_json = Of_Get_Menu();
                JObject json = JObject.Parse(ls_json);
                menu = json.ToString();
            }
            catch (Exception ex) { RegisterScript("dialogMsg('" + ex.Message + "')"); return; }
            string access_token = Get_Access_token(txt_appid.Text, txt_appsecret.Text);
            if (access_token == "")
            {
                RegisterScript("dialogMsg('Appid或AppSecret不对，请检查后再操作');");
                return;
            }
            string ls_rc = "";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);
            ls_rc = HttpPost(url, menu);
            JObject jo = JObject.Parse(ls_rc);
            if (jo["errmsg"].ToString() == "ok")
            {
                RegisterScript("dialogMsg('菜单更新成功');");
                //RegisterScript("localStorage.setItem('updatemenu" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', '" + menu + "');");
                if (chk_back.Checked)
                    DownloadTxtFile("微信公众号菜单备份-" + DateTime.Now.ToString("yyyyMMdd_HH:mm:ss") + ".txt", menu);
            }
            else
                RegisterScript("dialogMsg('菜单更新失败<br>" + jo["errmsg"].ToString() + "');");
        }
        /// <summary>
        /// 导出菜单
        /// </summary>
        protected void btn_backmenu_Click(object sender, EventArgs e)
        {
            string menu = "";
            try
            {
                string ls_json = Of_Get_Menu();
                JObject json = JObject.Parse(ls_json);
                menu = json.ToString();
            }
            catch { return; }
            if (menu != "")
                DownloadTxtFile("微信公众号菜单备份-" + DateTime.Now.ToString("yyyyMMdd_HH:mm:ss") + ".txt", menu);
            else
                RegisterScript("dialogMsg('请先拉取菜单或手动填入菜单选项');");
        }
        /// <summary>
        /// 从文件更新
        /// </summary>
        protected void btn_import_Click(object sender, EventArgs e)
        {
            string menu = "";
            try
            {
                string ls_json = txt_bak.Text.Trim();
                JObject json = JObject.Parse(ls_json);
                menu = json.ToString();
            }
            catch { RegisterScript("dialogMsg('菜单信息不正确,不是有效的json字符串')"); return; }
            string access_token = Get_Access_token(txt_appid.Text, txt_appsecret.Text);
            if (access_token == "")
            {
                RegisterScript("dialogMsg('Appid或AppSecret不对，请检查后再操作');");
                return;
            }
            string ls_rc = "";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);
            ls_rc = HttpPost(url, menu);
            JObject jo = JObject.Parse(ls_rc);
            if (jo["errmsg"].ToString() == "ok")
            {
                txt_bak.Text = "";
                RegisterScript("dialogMsg('菜单更新成功');");
                //RegisterScript("localStorage.setItem('updatemenu" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', '" + menu + "');");
                if (chk_back.Checked)
                    DownloadTxtFile("微信公众号菜单备份-" + DateTime.Now.ToString("yyyyMMdd_HH:mm:ss") + ".txt", menu);
            }
            else
                RegisterScript("dialogMsg('菜单更新失败<br>" + jo["errmsg"].ToString() + "');");
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        protected void btn_delmenu_Click(object sender, EventArgs e)
        {
            string access_token = Get_Access_token(txt_appid.Text, txt_appsecret.Text);
            if (access_token == "")
            {
                RegisterScript("dialogMsg('Appid或AppSecret不对，请检查后再操作');");
                return;
            }
            string ls_rc = "";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_token);
            ls_rc = HttpGet(url);
            JObject jo = JObject.Parse(ls_rc);
            if (jo["errmsg"].ToString() == "ok")
                RegisterScript("dialogMsg('公众号菜单已成功删除');");
            else
                RegisterScript("dialogMsg('菜单删除失败:<br>" + jo["errmsg"].ToString() + "');");
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
            if (Session["appid"] != null) return Session["appid"].ToString();
            string ls_appid = appid.Replace(" ", "");
            string ls_secret = appsecret.Replace(" ", "");
            string access_token = "";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", ls_appid, ls_secret);
            string ls_rc = HttpGet(url);
            JObject ja = JObject.Parse(ls_rc);
            try { access_token = ja["access_token"].ToString(); }
            catch { return ""; }
            Session["appid"] = access_token;
            return access_token;
        }
        /// <summary>
        /// HttpGet访问
        /// </summary>
        private string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string ret = string.Empty;
            Stream s;
            string StrDate = "";
            string strValue = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                s = response.GetResponseStream();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate;
                }
            }
            return strValue;
        }
        /// <summary>
        /// HttpPost访问
        /// </summary>
        private string HttpPost(string Url, string Params)
        {
            // 初始化WebClient  
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Accept-Language", "zh-cn");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //将字符串转换成字节数组
            byte[] postData = Encoding.GetEncoding("utf-8").GetBytes(Params);
            try
            {
                byte[] responseData = webClient.UploadData(Url, "POST", postData);
                string srcString = Encoding.GetEncoding("utf-8").GetString(responseData);
                return srcString.Trim();
            }
            catch (Exception Exce)
            {
                return "-1," + Exce.ToString() + "<br>" + Url + "    ";  // +Params;
            }
        }
        /// <summary>
        /// 向UI注册脚本
        /// </summary>
        private void RegisterScript(string msg)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "wtf", "<script>" + msg + "</script>");
        }
        #region 根据控件ID返回控件
        private TextBox GetTextBoxField(string name)
        {
            TextBox textbox = (TextBox)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
            return textbox;
        }
        private DropDownList GetDropDownListField(string name)
        {
            DropDownList ddlist = (DropDownList)this.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
            return ddlist;
        }
        #endregion
        #region 将控件中的数据转为json数据
        /// <summary>
        /// 将控件中的数据转为json数据
        /// </summary>
        private string Of_Get_Menu()
        {
            JObject json = new JObject();
            json["button"] = new JArray(new JObject[3] { new JObject(), new JObject(), new JObject() });

            #region 从控件获取最新菜单等待上传
            for (int i = 0; i < 3; i++)
            {
                string menu1 = (i + 1).ToString();
                json["button"][i]["name"] = GetTextBoxField("txt_Top" + menu1).Text.Trim();
                if (json["button"][i]["name"].ToString() == "")
                { if (i == 1) { RegisterScript("dialogMsg('第一个一级菜单名不能为空<br>请输入一级菜单名等其他信息<br>或点击拉取菜单获取服务器端的菜单。')"); return null; } }

                if (GetDropDownListField("DDL_Top" + menu1).SelectedValue.ToString() == "" || GetTextBoxField("txt_Key" + menu1).Text == "")
                {
                    #region 二级菜单
                    json["button"][i]["sub_button"] = new JArray(new JObject[5] { new JObject(), new JObject(), new JObject(), new JObject(), new JObject() });
                    for (int j = 0; j < 5; j++)
                    {
                        string menu2 = (j + 1).ToString();
                        json["button"][i]["sub_button"][j]["type"] = GetDropDownListField("DDL_Top" + menu1 + menu2).SelectedValue.ToString();
                        json["button"][i]["sub_button"][j]["name"] = GetTextBoxField("txt_Top" + menu1 + menu2).Text.Trim();
                        string type = "";
                        if (GetDropDownListField("DDL_Top" + menu1 + menu2).SelectedValue.ToString() == "view") { type = "url"; } else { type = "key"; }
                        json["button"][i]["sub_button"][j][type] = GetTextBoxField("txt_Key" + menu1 + menu2).Text.Trim();
                    }
                Top:
                    //删除空菜单
                    foreach (var a in json["button"][i]["sub_button"].Children())
                    {
                        if ((a["type"] == null ? "" : a["type"].ToString()) == "")
                        {
                            a.Remove();
                            goto Top;
                        }
                    }
                    #endregion
                }
                else
                {
                    json["button"][i]["type"] = GetDropDownListField("DDL_Top" + menu1).SelectedValue.ToString();
                    json["button"][i]["name"] = GetTextBoxField("txt_Top" + menu1).Text.Trim();
                    string type1 = "";
                    if (GetDropDownListField("DDL_Top" + menu1).SelectedValue.ToString() == "view") { type1 = "url"; } else { type1 = "key"; }
                    json["button"][i][type1] = GetTextBoxField("txt_Key" + menu1).Text.Trim();
                }
            }
            #endregion
            return json.ToString();
        }
        #endregion
        /// <summary>
        /// 下载TXT
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public void DownloadTxtFile(string fileName, string content)
        {
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(byteArray);
            Response.End();
        }

        #region 将视图状态保存到文件中
        /*
        //将视图状态保存到文件中
        //此方法应在Global.asax\Application_Start中调用
        private void CreateViewStateDirectory()
        {
            #region 建立文件夹保存视图状态的文件 并删除过期文件
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(this.Server.MapPath("~/App_Data/ViewState/"));
            if (!dir.Exists) dir.Create();
            else
            {
                DateTime nt = DateTime.Now.AddHours(-1);
                foreach (System.IO.FileInfo f in dir.GetFiles())
                {
                    if (f.CreationTime < nt) f.Delete();
                }
            }
            #endregion
        }
        protected override object LoadPageStateFromPersistenceMedium()
        {
            string viewStateID = (string)((Pair)base.LoadPageStateFromPersistenceMedium()).Second;
            string stateStr = (string)Cache[viewStateID];
            if (stateStr == null)
            {
                string fn = Path.Combine(this.Request.PhysicalApplicationPath, @"App_Data/ViewState/" + viewStateID);
                stateStr = File.ReadAllText(fn);
            }
            return new ObjectStateFormatter().Deserialize(stateStr);
        }
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            string value = new ObjectStateFormatter().Serialize(state);
            string viewStateID = (DateTime.Now.Ticks + (long)this.GetHashCode()).ToString(); //产生离散的id号码
            string fn = Path.Combine(this.Request.PhysicalApplicationPath, @"App_Data/ViewState/" + viewStateID);
            //ThreadPool.QueueUserWorkItem(File.WriteAllText(fn, value));
            File.WriteAllText(fn, value);
            Cache.Insert(viewStateID, value);
            base.SavePageStateToPersistenceMedium(viewStateID);
        }
        */
        //压缩ViewState并保存到session中
        protected override void SavePageStateToPersistenceMedium(object pageViewState)
        {
            MemoryStream ms = new MemoryStream();
            System.Web.UI.LosFormatter m_formatter = new System.Web.UI.LosFormatter();
            m_formatter.Serialize(ms, pageViewState);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string viewStateString = sr.ReadToEnd();
            byte[] ViewStateBytes = Convert.FromBase64String(viewStateString);
            ViewStateBytes = ViewStateCompression.Compress(ViewStateBytes);
            Session["__ViewState"] = Convert.ToBase64String(ViewStateBytes);
            ms.Close();
            return;
        }
        // 序列化ViewState
        protected override object LoadPageStateFromPersistenceMedium()
        {
            object viewStateBag;
            string m_viewState = (string)Session["__ViewState"];
            byte[] ViewStateBytes = Convert.FromBase64String(m_viewState);
            ViewStateBytes = ViewStateCompression.Decompress(ViewStateBytes);
            System.Web.UI.LosFormatter m_formatter = new System.Web.UI.LosFormatter();
            try
            {
                viewStateBag = m_formatter.Deserialize(Convert.ToBase64String(ViewStateBytes));
            }
            catch (Exception ex)
            {
                //Log.Insert( "页面Viewtate是空." );
                viewStateBag = string.Empty;
            }
            return viewStateBag;
        }

        public class ViewStateCompression
        {
            public ViewStateCompression()
            {
                //
                // TODO: Add constructor logic here
                //
            }
            // 压缩
            public static byte[] Compress(byte[] data)
            {
                MemoryStream output = new MemoryStream();
                System.IO.Compression.GZipStream gzip =
                    new System.IO.Compression.GZipStream(output, System.IO.Compression.CompressionMode.Compress, true);
                gzip.Write(data, 0, data.Length);
                gzip.Close();
                return output.ToArray();
            }
            // 解压缩
            public static byte[] Decompress(byte[] data)
            {
                MemoryStream input = new MemoryStream();
                input.Write(data, 0, data.Length);
                input.Position = 0;
                System.IO.Compression.GZipStream gzip = 
                    new System.IO.Compression.GZipStream(input, System.IO.Compression.CompressionMode.Decompress, true);
                MemoryStream output = new MemoryStream();
                byte[] buff = new byte[64];
                int read = -1;
                read = gzip.Read(buff, 0, buff.Length);
                while (read > 0)
                {
                    output.Write(buff, 0, read);
                    read = gzip.Read(buff, 0, buff.Length);
                }
                gzip.Close();
                return output.ToArray();
            }
        }
        #endregion
    }
}