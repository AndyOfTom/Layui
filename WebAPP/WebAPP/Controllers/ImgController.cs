using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPP.Models;
using Newtonsoft.Json;
using DataBase;

namespace WebAPP.Controllers
{
    public class ImgController : Controller
    {
        // GET: Img
        public ActionResult UpLoadList()
        {
            return View();
        }
        public ActionResult UpLoadList1()
        {
            return View();
        }
        public ActionResult getType()
        {
            string sql = " SELECT A.Type ,B.TypeName FROM IMGLIST A  ";
            sql += " JOIN ImgType B ";
            sql += " ON A.Type=B.TypeId WHERE A.UserID=1 ";
            sql += " GROUP BY A.Type,B.TypeName ";
            var dataset = JsonConvert.SerializeObject(DataBase.SqlDbHelper.ExecuteDataTable(sql));
            return Json(dataset);
        }
        public ActionResult Uploadimg()
        {
            return View();
        }
      
        public int CreateImg(string type)
        {
            var files = Request.Files.Count;
            int code = 0;
           


                var path = Server.MapPath("/Content/Img/") + "$" + DateTime.Now.ToString("yyyyMMddHHmmss") + "#" + Request.Files[0].FileName;

                Request.Files[0].SaveAs(path);
                string sql = "INSERT INTO IMG values( ";
                sql += "'" + Request.Files[0].FileName + "',";
                sql += "'" + "/Content/Img/" + path.Substring(path.IndexOf('$') - 1) + "',";
                sql += "'',";
                sql += "'" + type + "',";
                sql += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
                code = DataBase.SqlDbHelper.ExecuteNonQuery(sql);
            

            return code;
        }
        public ActionResult ImgListTable()
        {

            return View();
        }
        public int Upload(string name, string type, string desc)
        {
            // var User = (User_Info)Session["User"];
            //var UserID = User.Id;
            var file = Request.Files[0];
            var path = Server.MapPath("/Content/Img/") + "$" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*" + file.FileName;
            file.SaveAs(path);
            string sql = " INSERT INTO IMGLIST VALUES( ";
            sql += "'" + name + "',";
            sql += "'" + desc + "',";
            sql += "'" + type + "',";
            sql += "'" + 20 + "',";

            sql += "'" + "/Content/Img/" + path.Substring(path.IndexOf('$') - 1) + "',";
            sql += "'" + 1 + "',";
            sql += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
            int code = DataBase.SqlDbHelper.ExecuteNonQuery(sql);
            return code;
        }

        public int DelImgList(int id)
        {
            string sql = " DELETE * FROM IMGLIST WHERE ID = " + id;
            return DataBase.SqlDbHelper.ExecuteNonQuery(sql);
        }
        public string DelTable(string id)
        {

            string ids = id.Substring(0, id.Length - 1);
            string sql = "DELETE FROM Img WHERE ImgListID in( " + ids+" )";
            sql += "; DELETE FROM IMGLIST WHERE ID  in( " + ids+ " )";
            var i = SqlDbHelper.ExecuteNonQuery(sql);
            var msg = "";
            if (i > 0)
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }

          
            return msg;
        }
        public ActionResult CreatrSuccess(int? id)
        {
            string msg = "";
            if (id == 1)
            {
                msg = "上传相册成功!";
            }
            else
            {
                msg = "上传照片成功!";
            }
            ViewData["msg"] = msg;
            return View();
        }


        public string GetTables(int Page=1,int limit=10) {

            int userID = 1;
            string sql = "   SELECT * FROM( ";
            sql += " SELECT * FROM ( ";
            sql += " SELECT  ROW_NUMBER() OVER(order by CreateTime) as row_number,* from( ";
            sql += " SELECT A.Id,COUNT(B.ImgListID) as ImgCount,C.TypeName,MAX(A.Fisrt) as fisrtImg,A.Comment,MAX(A.CreateTime) createtime,A.Name FROM ImgList A";
            sql += " LEFT JOIN Img B ON A.Id=B.ImgListID ";
            sql += " LEFT JOIN ImgType C ON A.Type=C.TypeId ";
            sql += " WHERE A.USERID=" + userID + "GROUP BY A.Id,C.TypeName,A.Comment,A.Name ";
            sql += " ) X ) M  WHERE M.row_number  BETWEEN " + ((Page - 1) * limit + 1) + " AND " + (Page) * limit;
            sql += ") XD RIGHT JOIN  (SELECT COUNT(1)AS COUNTNUM FROM ImgList H WHERE H.UserID=1) XC ON 1=1 ";
            var list = SqlDbHelper.ExecuteDataTable(sql);
            var datalist = new List<DataItem>();
            for (int i = 0; i < list.Rows.Count; i++)
            {

                DataItem item = new DataItem();
                item.Comment = list.Rows[i]["Comment"].ToString();
                item.COUNTNUM = Convert.ToInt32(list.Rows[i]["COUNTNUM"].ToString());
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
            return JsonConvert.SerializeObject(root);
        }
    }


}