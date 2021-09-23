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
    public class PeopleController : Controller
    {

        public ActionResult QueryAll()
        {
            List<People> _data = new List<People>();

            using (SqlDataBase db = new SqlDataBase())
            {
                string _sql = $"SELECT TOP (1000) [id] ,[name]  ,[phone_number] FROM[paulsql].[dbo].[Table_1]";
                DataTable _dt = db.ToDataTable(_sql);

                _data = DataTableHelper.ToList<People>(_dt).ToList();
            }
            return View(_data);
        }

        [HttpPost]
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
        }

    }
}