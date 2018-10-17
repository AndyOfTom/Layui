using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPP.Models;

namespace WebAPP.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserInfo()
        {
            return View();
        }
        public ActionResult UpdatePwd() {

            return View();
        }
        public ActionResult UpdatePwdInfo(string Pwd1, string Pwd2)
        {
            var User = (User_Info)Session["User"];
            var UserID = User.Id;
            string ProcName = "UPDATE_USER_PWD";
            SqlParameter[] para = { new SqlParameter ( "@P_ID", SqlDbType.Int ),
            new SqlParameter ( "@P_NewPWD", SqlDbType.VarChar ),
            new SqlParameter ( "@P_OldPWD", SqlDbType.VarChar ),
            new SqlParameter ( "@P_CODE",SqlDbType.VarChar ) };
            para[0].Value = UserID;
            para[0].Direction = ParameterDirection.Input;
            para[1].Value = Pwd2;
            para[1].Direction = ParameterDirection.Input;
            para[2].Value = Pwd1;
            para[2].Direction = ParameterDirection.Input; //设定参数的输出方向  
            para[3].Value = 0;
            para[3].Direction = ParameterDirection.Output; //设定参数的输出方向 
            string[] strarray = { "@P_CODE"};
            var str = SqlDbHelper.ExceProc(ProcName, para, strarray);
            return Json(new { data=(str[0].ToString()==null)?null:str[0].ToString()});
        }
       
        public ActionResult UpdataUser(string loginname, DateTime time, string InPut, string job, int sex, string address)
        {
            var User = (User_Info)Session["User"];
            var UserID = User.Id;
            string sql = "UPDATE USER_INFO SET Name=@NAME,Bartime=@time,Input=@InPut,job=@JOB,Sex=@Sex,Address=@address WHERE Id=@UserID ";
            SqlParameter[] sp = {new SqlParameter("@UserID", UserID),
                new SqlParameter("@NAME", loginname),
                new SqlParameter("@time", time),
                new SqlParameter("@InPut", InPut),
                new SqlParameter("@JOB", job),
                new SqlParameter("@Sex", sex),
                new SqlParameter("@address", address)

            };
            if (SqlDbHelper.ExecuteNonQuery(sql, sp)>0)
            {
                return Json(new { Msg="提交成功"});
            }

            return Json(new { Msg = "提交失败" });
        }
    }
}