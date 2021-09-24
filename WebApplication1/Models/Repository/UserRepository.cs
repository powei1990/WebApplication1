using MyGis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Repository
{
    public class UserRepository
    {
        private string _sql = string.Empty;

        public List<People> QueryPeople(People query)
        {
            List<People> _data = new List<People>();
            List<string> _whr = new List<string>();
            List<SqlParameter> _par = new List<SqlParameter>();
            
            if (!string.IsNullOrEmpty(query.name))
            {
                _whr.Add("[name] like '%' + @name + '%'");
                _par.Add(new SqlParameter("@name", query.name));
            }
            if (!string.IsNullOrEmpty(query.phone_number))
            {
                _whr.Add("[phone_number] like '%' + @phone_number + '%'");
                _par.Add(new SqlParameter("@phone_number", query.phone_number));
            }
           

            using (SqlDataBase db = new SqlDataBase())
            {
                string _table = "[Table_1]";
                _sql = $"select * from {_table} ";
                if (_whr.Count > 0)
                {
                    string _whrStr = string.Join(" and ", _whr.ToArray());
                    _sql += $"where {_whrStr} ";
                }

                DataTable _dt = db.ToDataTable(_sql, _par.ToArray());

                _data = DataTableHelper.ToList<People>(_dt).ToList();
            }

            return _data;
        }

        public bool AddPeople(People adddata)
        {
            bool _flag = false;
            using (SqlDataBase db = new SqlDataBase())
            {
                List<string> _col = new List<string>();
                List<SqlParameter> _par = new List<SqlParameter>();

                //_col.Add("id");
                //_par.Add(new SqlParameter("@id", adddata.id));
                _col.Add("name");
                _par.Add(new SqlParameter("@name", adddata.name));
                _col.Add("phone_number");
                _par.Add(new SqlParameter("@phone_number", adddata.phone_number));


                string _table = "[Table_1]";
                //string _titleStr = string.Join(", ", _col.Select(c => { c = $"[{c}]"; return c; }).ToArray());
                //這段是啥Lamda?
                string _valueStr = string.Join(", ", _col.Select(c => { c = $"@{c}"; return c; }).ToArray());

                //_sql = $"insert into {_table} ([id], {_titleStr}) ";
                //_sql += $"select isnull(max([id]), 0) + 1, {_valueStr} from {_table} ";
                _sql = $" insert into {_table} values((select MAX(id) + 1 from {_table}) ,{_valueStr})";
                _flag = db.ToExecute(_sql, _par.ToArray());
            }

            return _flag;
        }
    }
}