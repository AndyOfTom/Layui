using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebAPP.Models;
using System.Net;
using System.Text;
using System.IO;

namespace WebAPP.Controllers
{

    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult AccordionIndex()
        {

            /*var user = (User_Info)Session["User"];
            int userID = user.Id;*/
            int UserId = 1;
            string sql = "SELECT * FROM USER_INFO WHERE ID=" + UserId;
            var UserTable = SqlDbHelper.ExecuteDataTable(sql);
            User_Info user = new User_Info();
            user.Id = Convert.ToInt32(UserTable.Rows[0]["Id"].ToString());
            user.Address = UserTable.Rows[0]["Address"].ToString();
            user.Bartime = DateTime.Parse(UserTable.Rows[0]["Bartime"].ToString());
            user.CreateDate = DateTime.Parse(UserTable.Rows[0]["CreateDate"].ToString());
            user.Email = UserTable.Rows[0]["Email"].ToString();
            user.Input = UserTable.Rows[0]["Input"].ToString();
            user.IsAction = UserTable.Rows[0]["IsAction"].ToString();
            user.IsAdmin = UserTable.Rows[0]["IsAdmin"].ToString();
            user.LoginName = UserTable.Rows[0]["LoginName"].ToString();
            user.UpdateDate = DateTime.Parse(UserTable.Rows[0]["UpdateDate"].ToString());
            user.Spell = UserTable.Rows[0]["Spell"].ToString();
            user.Sex = UserTable.Rows[0]["Sex"].ToString();
            user.Spell = UserTable.Rows[0]["Spell"].ToString();
            user.QQ = UserTable.Rows[0]["QQ"].ToString();
            user.Phone = UserTable.Rows[0]["Phone"].ToString();
            user.PassWrod = UserTable.Rows[0]["PassWrod"].ToString();
            user.Name = UserTable.Rows[0]["Name"].ToString();
            user.job = UserTable.Rows[0]["job"].ToString();
            Session["User"] = user;
            ViewData["UserName"] = user.LoginName;
            return View();
        }

        public ActionResult AccordionPage()
        {
            return View();
        }
        public ActionResult AccordionTest()
        {
            return View();
        }
        public ActionResult AccordionTest2()
        {
            return View();
        }

        public ActionResult WebApp()
        {

            return View();
        }
        public ActionResult ImgList()
        {
            return View();
        }
        /// <summary>
        /// 获取页数的相册
        /// </summary>
        /// <param name="Page">当前页</param>
        /// <returns></returns>
        /// 

        public string GetPageImgList(int Page = 1, int limit=0)
        {
            /*var user = (User_Info)Session["User"];
            int userID = user.Id;*/
            int userID = 1;
            string sql = "   SELECT * FROM( ";
            sql += " SELECT * FROM ( ";
            sql += " SELECT  ROW_NUMBER() OVER(order by CreateTime) as row_number,* from( ";
            sql += " SELECT A.Id,COUNT(B.ImgListID) as ImgCount,C.TypeName,MAX(A.Fisrt) as fisrtImg,A.Comment,MAX(A.CreateTime) createtime,A.Name FROM ImgList A";
            sql += " LEFT JOIN Img B ON A.Id=B.ImgListID ";
            sql += " LEFT JOIN ImgType C ON A.Type=C.TypeId ";
            sql += " WHERE A.USERID=" + userID + "GROUP BY A.Id,C.TypeName,A.Comment,A.Name ";
            sql += " ) X ) M  WHERE M.row_number  BETWEEN " + ((Page-1)* 8+1 ) + " AND " + (Page) * 8;
            sql += ") XD RIGHT JOIN  (SELECT COUNT(1)AS COUNTNUM FROM ImgList H WHERE H.UserID=1) XC ON 1=1 ";
            var list = SqlDbHelper.ExecuteDataTable(sql);

           
            var pagelist = JsonConvert.SerializeObject(list);
            return pagelist;
        }
        public ActionResult PutPageImgList(int Page = 1, int limit = 10)
        {
            /*var user = (User_Info)Session["User"];
            int userID = user.Id;*/
            int userID = 1;
            string sql = "   SELECT * FROM( ";
            sql += " SELECT * FROM ( ";
            sql += " SELECT  ROW_NUMBER() OVER(order by CreateTime) as row_number,* from( ";
            sql += " SELECT A.Id,COUNT(B.ImgListID) as ImgCount,C.TypeName,MAX(A.Fisrt) as fisrtImg,A.Comment,MAX(A.CreateTime) createtime,A.Name FROM ImgList A";
            sql += " LEFT JOIN Img B ON A.Id=B.ImgListID ";
            sql += " LEFT JOIN ImgType C ON A.Type=C.TypeId ";
            sql += " WHERE A.USERID=" + userID + "GROUP BY A.Id,C.TypeName,A.Comment,A.Name ";
            sql += " ) X ) M  WHERE M.row_number  BETWEEN " + ((Page-1)* limit + 1) + " AND " + (Page ) * limit;
            sql += ") XD RIGHT JOIN  (SELECT COUNT(1)AS COUNTNUM FROM ImgList H WHERE H.UserID=1) XC ON 1=1 ";
            var list = SqlDbHelper.ExecuteDataTable(sql);
            var datalist =new List<DataItem>();
            for (int i = 0; i < list.Rows.Count; i++)
            {

                DataItem item = new DataItem();
                item.Comment = list.Rows[i]["Comment"].ToString();
                item.COUNTNUM =Convert.ToInt32(list.Rows[i]["COUNTNUM"].ToString());
                item.createtime = list.Rows[i]["createtime"].ToString();
                item.TypeName = list.Rows[i]["TypeName"].ToString();
                item.Name = list.Rows[i]["Name"].ToString();
                item.Id = Convert.ToInt32(list.Rows[i]["Id"].ToString());
                item.fisrtImg = list.Rows[i]["fisrtImg"].ToString();
                item.ImgCount = Convert.ToInt32(list.Rows[i]["ImgCount"].ToString());
                item.row_number = Convert.ToInt32(list.Rows[i]["row_number"].ToString());
                datalist.Add(item);
            }
            Root root = new Root();
            root.code = 0;
            root.count = datalist.FirstOrDefault().COUNTNUM;
            root.msg = "";
            root.data = datalist;
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        public string GetPageImg(int id=1) {

            /*var user = (User_Info)Session["User"];
             int userID = user.Id;*/
            int UserId = 1;

            string sql = "";
            sql += " SELECT A.Id,A.Name,A.Src,A.Comment,A.ImgListID AS ImgID,A.UploadTime FROM IMG A  ";
            sql += " JOIN ImgList B ON A.ImgListID=B.Id  ";
            sql += " WHERE B.UserID= " + UserId;
            sql += " AND A.IMGLISTID=" + id ;
          
            var list = SqlDbHelper.ExecuteDataTable(sql);
            var pagelist = JsonConvert.SerializeObject(list);
            return pagelist;
        }
    public ActionResult ImgListModel(int id)
    {


            ViewData["listid"] = id;

            return View();
        }
    
    [HttpPost]
    public ActionResult GetServer(string posturl, string data)
    {
        WebRequest request = WebRequest.Create(posturl);
        request.Method = "POST";
        request.Timeout = 10000;
        byte[] byteArray = Encoding.UTF8.GetBytes(data);

        //设置request的MIME类型及内容长度
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;

        //打开request字符流
        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        WebResponse response = request.GetResponse();

        //获取相应的状态代码
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        //定义response字符流
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();//读取所有
                                                       //定义response为前面的request响应
        return Json(new { Msg = responseFromServer });
    }

    public ActionResult ShortcutsListJson()
    {

        return Content("");
    }
    /*
      "title": "武斌博客",
    "icon": "fa-cubes",
    "spread": true,
    "href": "",
         */

    public ActionResult LoadAccordionMenu()
    {
        // string ObjectId = ManageProvider.Provider.Current().ObjectId;
        //  List<Base_Module> list = base_modulepermissionbll.GetModuleList(ObjectId).FindAll(t => t.Enabled == 1);

        var datajson = "";
        var cache = HttpRuntime.Cache;

        try
        {
            if (cache.Get("Men") == null)
            {
                string sql = "SELECT * FROM Men";
                DataTable data = SqlDbHelper.ExecuteDataTable(sql);



                datajson = JsonConvert.SerializeObject(data);

                HttpRuntime.Cache.Insert("Men", datajson, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
            }
            return Content(cache.Get("Men").ToString());
        }
        catch (Exception)
        {

            throw;
        }
    }
}
}