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
using WebApplication1.Models.Repository;

namespace WebApplication1.Controllers
{
    public class PeopleController : Controller
    {
        //會先按照controller名稱對應的資料夾尋找QueryAll.cshtml
        public ActionResult QueryAll()
        {
            List<People> _data = new List<People>();

            using (SqlDataBase db = new SqlDataBase())
            {
                string _sql = $"SELECT TOP (1000) [id] ,[name]  ,[phone_number] FROM[paulsql].[dbo].[Table_1]";
                DataTable _dt = db.ToDataTable(_sql);

                _data = DataTableHelper.ToList<People>(_dt).ToList();
            }
            //這邊會回覆View然後把_data資料進去
            return View(_data);
        }

        [HttpPost]
        public ActionResult QueryAll(People data)
        {
            ViewBag.Message = "Your QueryAll page.";
            UserRepository _repository = new UserRepository();
            List<People> _data = _repository.QueryPeople(data);

            return Content(JsonConvert.SerializeObject(_data), "application/json");
        }


        [HttpPost]
        public ActionResult AddPeople(People adddata)
        {
            bool _flag = false;
            UserRepository _repository = new UserRepository();
            _flag = _repository.AddPeople(adddata);

            return Json(_flag);
        }


        [HttpPost]
        public ActionResult DeletePeople(string id)
        {
            UserRepository _repository = new UserRepository();
            bool _flag = _repository.DelData(id);

            return Content(_flag.ToString().ToLower());
        }

        public ActionResult EditPeople(People editdata)
        {
            UserRepository _repository = new UserRepository();
            bool  _flag = _repository.EditPeople(editdata);

            return Json(_flag);
        }

    }
}