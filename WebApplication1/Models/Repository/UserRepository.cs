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

        public List<People> getList(People query)
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
    }
}