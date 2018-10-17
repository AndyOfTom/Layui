using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebAPP.Models;
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace Api.Controllers
{
   
    public class TableController : ApiController
    {

        //SELECT 
        public HttpResponseMessage GetTable(int Page = 1, int limit = 10)
        {

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
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(root), Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        //DELETE
        [HttpGet]
        public HttpResponseMessage DelTable (int id) {
            string sql = "DELETE * FROM Img WHERE ImgListID="+id ;
            sql += "; DELETE * FROM IMGLIST WHERE ID ="+id;
           var i= SqlDbHelper.ExecuteNonQuery(sql);
            var msg = "";
            if (i > 0)
            {
                msg = "删除成功";
            }
            else {
                msg = "删除失败";
            }

            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(msg, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

    }
}
