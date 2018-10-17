using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebAPP.Models;
using System.Web.Caching;

namespace WebAPP.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Account">用户账号</param>
        /// <param name="Password">用户密码</param>
        /// <returns></returns>
        public int CheckLogin(string Account, string Password)
        {
            string ProcName = "USER_LOGIN";

            SqlParameter[] para ={
                                     new SqlParameter("@LOGIN_NAME",SqlDbType.VarChar),
                                     new SqlParameter("@PASSWORD",SqlDbType.VarChar),
                                     new SqlParameter("@P_CODE",SqlDbType.VarChar),
                                      new SqlParameter("@P_ID",SqlDbType.VarChar),
            };
            para[0].Value = Account;
            para[0].Direction = ParameterDirection.Input;
            para[1].Value = Password;
            para[1].Direction = ParameterDirection.Input;
            para[2].Value = 0;
            para[2].Direction = ParameterDirection.Output; //设定参数的输出方向  
            para[3].Value = 0;
            para[3].Direction = ParameterDirection.Output; //设定参数的输出方向  
            string[] outstr = { "@P_CODE", "@P_ID" };
            var str = SqlDbHelper.ExceProc(ProcName, para, outstr);
            Session["UserID"] = str[1].ToString();
            return int.Parse(str[0].ToString());
        }

        //register
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>

        public ActionResult RegisterSuccess()
        {

            return View();
        }
        public ActionResult RegisterToSql(string pname, string ppwd, string pph, string pcode, string peamil)
        {

            var cache = HttpRuntime.Cache;
            var code = cache.Get(pph);
            if (code == null)
            {
                return Json(new { data = 3, msg = "验证码已失效" });
            }
            else if (code.ToString() != pcode)
            {
                return Json(new { data = 1, msg = "验证码不正确" });
            }
            else
            {
                string sql = "INSERT INTO User_Info Values (@Id,@Name,@PassWrod,@Phone,@Email,@CreateDate,@UpdateDate,@IsAdmin,@IsAction,@QQ,@Spell,@LoginName,@Address,@job,@Bartime,@Sex,@Input) ";
                var guid = new Guid();
                SqlParameter[] sp = new SqlParameter[] {
                  new SqlParameter("@Id",guid),
                  new SqlParameter("@Name",""),
                  new SqlParameter("@PassWrod",ppwd),
                  new SqlParameter("@Phone",pph),
                  new SqlParameter("@Email",peamil),
                  new SqlParameter("@CreateDate",DateTime.Now),
                  new SqlParameter("@UpdateDate",DateTime.Now),
                  new SqlParameter("@IsAdmin",'0'),
                  new SqlParameter("@IsAction",'0'),
                  new SqlParameter("@QQ",""),
                  new SqlParameter("@Spell",""),
                  new SqlParameter("@Address",""),
                  new SqlParameter("@job",""),
                  new SqlParameter("@Bartime",""),
                  new SqlParameter("@Sex",""),
                  new SqlParameter("@Input",""),

                  };
                var i = SqlDbHelper.ExecuteNonQuery(sql, sp); ;

                if (i > 0)
                {
                    return View("RRegisterSuccess");
                }
                else
                {
                    return Json(new { data = 2, msg = "发生未知错误" });

                }

            }
        }
        /// <summary>
        /// 判断用户名是否被注册
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns>
        [HttpGet]
        public int RegisterOnchange(string UserName)
        {
            string sql = "SELECT 1 FROM User_Info A WHERE A.LoginName=@UserName";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@LoginName",UserName)
            };
            var DateSet = SqlDbHelper.GetDataSet(sql, "User", sp);
            if (DateSet.Tables[0].Rows.Count == 0)
            {

                return 0;
            }
            return 1;
        }
        object lockpost = new object();
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="txtph">手机号</param>
        /// <returns></returns>
        public JsonResult SendPost(string txtph)
        {

            string url = "http://106.ihuyi.cn/webservice/sms.php?method=Submit";

            try
            {
                lock (lockpost)
                {
                    string account = "C10744603";//用户名是登录用户中心->验证码、通知短信->帐户及签名设置->APIID
                    string password = "268a0968d90a3e273d96d53f0ea5999a"; //密码是请登录用户中心->验证码、通知短信->帐户及签名设置->APIKEY

                    Random rad = new Random();
                    int mobile_code = rad.Next(10000, 999999);

                    string content = "您的验证码是：" + mobile_code + " 。请不要把验证码泄露给其他人。";

                    //Session["mobile"] = mobile;
                    //Session["mobile_code"] = mobile_code;

                    string postStrTpl = "account={0}&password={1}&mobile={2}&content={3}";
                    HttpWebRequest httpWeb = (HttpWebRequest)WebRequest.Create(url);
                    httpWeb.Timeout = 10000;
                    httpWeb.Method = "post";
                    httpWeb.ContentType = "application/x-www-form-urlencoded";
                    var DataByte = Encoding.UTF8.GetBytes(string.Format(postStrTpl, account, password, txtph, content));
                    httpWeb.ContentLength = DataByte.Length;
                    var StreamData = httpWeb.GetRequestStream();
                    StreamData.Write(DataByte, 0, DataByte.Length);
                    WebResponse response = httpWeb.GetResponse();

                    //获取相应的状态代码


                    //定义response字符流
                    var dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();//读取所有


                    if (((HttpWebResponse)response).StatusDescription == "OK")
                    {
                        int len1 = responseFromServer.IndexOf("</code>");
                        int len2 = responseFromServer.IndexOf("<code>");
                        string code = responseFromServer.Substring((len2 + 6), (len1 - len2 - 6));
                        if (code == "2")
                        {

                            return Json(new { state = code, SendTime = DateTime.Now });
                        }

                        return Json(new { state = code, SendTime = DateTime.Now });
                    }
                    return Json(new { state = "500", code = "发送短信失败,网络异常" });


                }
            }
            catch (Exception ex)
            {

                return Json(new { data = 3, msg = ex.ToString() });
            }
        }
        /// <summary>
        /// 将短信接口信息拼接
        /// </summary>
        /// <param name="sp">数组</param>
        /// <returns></returns>
        public string GetData(IDictionary<string, string> sp)
        {
            IEnumerator<KeyValuePair<string, string>> dem = sp.GetEnumerator();
            StringBuilder SB = new StringBuilder();
            while (dem.MoveNext())
            {
                string Key = dem.Current.Key;
                string Value = dem.Current.Value;
                var Values = HttpUtility.UrlEncode(Value, Encoding.GetEncoding("utf-8"));
                SB.Append(Key + "=" + Values + "&");
            }
            return SB.ToString(); ;
        }
        /// <summary>
        /// 获取随机验证码
        /// </summary>
        /// <returns>返回6位验证码</returns>
        public int GrtCode()
        {
            Random r = new Random();
            return r.Next(100000, 999999);
        }



    }

}