using MyGis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /*
        public ActionResult QueryAll(People data)
        {
            ViewBag.Message = "Your QueryAll page.";

            List<People> _data = new List<People>();
            List<string> _whr = new List<string>();
            List<SqlParameter> _par = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(data.name))
            {
                _whr.Add("[name] like '%' + @name + '%'");
                _par.Add(new SqlParameter("@name", data.name));
            }
            using (SqlDataBase db = new SqlDataBase())
            {
                string _sql = $"SELECT TOP (1000) [id] ,[name]  ,[phone_number] FROM[paulsql].[dbo].[Table_1]";
                DataTable _dt = db.ToDataTable(_sql);

                _data = DataTableHelper.ToList<People>(_dt).ToList();
            }

            return Content(JsonConvert.SerializeObject(_data), "application/json");
            //return View();
        }
        
        public ActionResult NewsSearch(People data)
        {
                List<People> _data = new List<People>();
            List<string> _whr = new List<string>();
            List<SqlParameter> _par = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(data.name))
            {
                _whr.Add("[name] like '%' + @name + '%'");
                _par.Add(new SqlParameter("@name", data.name));
            }
            using (SqlDataBase db = new SqlDataBase())
                {
                    string _sql = $"SELECT TOP (1000) [id] ,[name]  ,[phone_number] FROM[paulsql].[dbo].[Table_1]";
                    DataTable _dt = db.ToDataTable(_sql);

                    _data = DataTableHelper.ToList<People>(_dt).ToList();
                }

            return Content(JsonConvert.SerializeObject(_data), "application/json");
        }
        public class News
        {
            public string text { get; set; }

            public string val { get; set; }
        }
        */
    }
}