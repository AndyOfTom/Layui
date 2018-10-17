using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPP.Models
{
    public class ImgList
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public object Type { get; set; }
        public int Size { get; set; }
        public int Fisrt { get; set; }
        public int UserID { get; set; }

    }
}