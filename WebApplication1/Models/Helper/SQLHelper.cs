using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyGis
{
    public class GridSetting
    {
        /// <summary>
        /// 升降序排列類型,預設ASC
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// 升序(由小到大)
            /// </summary>
            Asc,
            /// <summary>
            /// 降序(由大到小)
            /// </summary>
            Desc
        }

        /// <summary>
        /// 分頁方法SQL2012以後才有Offset_Fetch,預設Row_Number
        /// </summary>
        public enum SortPage
        {
            /// <summary>
            /// 升序(由小到大)
            /// </summary>
            Row_Number,
            /// <summary>
            /// 降序(由大到小)
            /// </summary>
            Offset_Fetch
        }

        /// <summary>
        /// 顯示頁碼
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 顯示筆數
        /// </summary>
        public int PageSize { get; set; } = 10;


        public int BeginRow
        {
            get
            {
                return (PageIndex - 1) * PageSize + 1;
            }
        }

        public int EndRow
        {
            get
            {
                return PageIndex * PageSize;
            }
        }

        /// <summary>
        /// 排序欄位
        /// </summary>
        public String SortColumn { get; set; }

        /// <summary>
        /// 升降序類型
        /// </summary>
        public SortType SortOrder { get; set; } = SortType.Asc;

        public string StrSortOrder
        {
            set
            {
                if ("Desc".Equals(value))
                {
                    SortOrder = SortType.Desc;
                }
                else
                {
                    SortOrder = SortType.Asc;
                }
            }

        }

        /// <summary>
        /// 分頁排序版本
        /// </summary>
        public SortPage SortVersion { get; set; } = SortPage.Row_Number;

        /// <summary>
        /// 取得分頁總頁數
        /// </summary>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public int GetTotalPage(int totalCount)
        {
            return (int)Math.Ceiling((double)totalCount / this.PageSize);
        }
    }

    /// <summary>
    /// clsDB 的摘要描述
    /// </summary>
    public class SqlDataBase : IDisposable
    {
        private SqlConnection objConn;
        public enum ConnStrNameEnum { DBConnection }


        public SqlDataBase()
        {
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStrNameEnum.DBConnection.ToString()].ToString());
        }

        public SqlDataBase(ConnStrNameEnum ConnStrName)
        {
            objConn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnStrName.ToString()].ToString());
        }

        /// <summary>
        /// 關閉連線
        /// </summary>
        public void Dispose()
        {
            if (objConn.State != ConnectionState.Closed)
            {
                objConn.Close();
            }
        }

        /// <summary>
        /// 執行語法
        /// </summary>
        /// <param name="sql">語法</param>
        /// <param name="sqlParams">參數</param>
        /// <returns>return bool</returns>
        public bool ToExecute(string sql, SqlParameter[] sqlParams = null)
        {
            bool result = true;
            SqlCommand objCmd = new SqlCommand(sql, objConn);

            if (objConn.State != ConnectionState.Open)
            {
                objConn.Open();
            }

            if (sqlParams != null)
            {
                objCmd.Parameters.AddRange(sqlParams);
            }

            objCmd.ExecuteNonQuery();

            return result;
        }

        /// <summary>
        /// 回傳單筆資料
        /// </summary>
        /// <param name="sql">語法</param>
        /// <param name="sqlParams">參數</param>
        /// <returns>return string</returns>
        public string GetResult(string sql, SqlParameter[] sqlParams = null)
        {
            SqlCommand objCmd = new SqlCommand(sql, objConn);

            if (objConn.State != ConnectionState.Open)
            {
                objConn.Open();
            }

            if (sqlParams != null)
            {
                objCmd.Parameters.AddRange(sqlParams);
            }

            string objStr = objCmd.ExecuteScalar().ToString();

            return objStr;
        }

        /// <summary>
        /// 回傳資料表
        /// </summary>
        /// <param name="sql">語法</param>
        /// <param name="sqlParams">參數</param>
        /// <returns>return DataTable</returns>
        public DataTable ToDataTable(string sql, SqlParameter[] sqlParams = null)
        {
            DataTable objTab = new DataTable();

            SqlCommand objCmd = new SqlCommand(sql, objConn);

            if (sqlParams != null)
            {
                objCmd.Parameters.AddRange(sqlParams);
            }

            SqlDataAdapter objDa = new SqlDataAdapter(objCmd);
            objDa.Fill(objTab);

            return objTab;
        }

        /// <summary>
        /// 回傳資料表(分頁)
        /// </summary>
        /// <param name="sql">語法</param>
        /// <param name="sqlParams">參數</param>
        /// <param name="gridSetting">分頁</param>
        /// <param name="totalCount">回傳總比數</param>
        /// <returns>return DataTable</returns>
        public DataTable ToDataTable(string sql, SqlParameter[] sqlParams, GridSetting gridSetting, out int totalCount)
        {
            DataTable objTab = new DataTable();

            string _sql = sql;

            if (gridSetting != null)
            {
                if (gridSetting.SortVersion == GridSetting.SortPage.Row_Number)
                {
                    _sql = $"with [T] as (select row_number() over (order by [{gridSetting.SortColumn}] {gridSetting.SortOrder.ToString()}) as [RowIndex],* from ({sql}) as [Table]), ";
                    _sql += $"[T2] as (select count(1) as [TotalCount] FROM [T]) ";
                    _sql += $"select * from [T2], [T] where [RowIndex] between ({gridSetting.PageIndex} - 1) * {gridSetting.PageSize} + 1 and {gridSetting.PageIndex} * {gridSetting.PageSize} ";
                }
                else
                {
                    _sql = $"with [T] as (select ,* from ({sql}) as [Table]), ";
                    _sql += $"[T2] as (select count(1) as [TotalCount] FROM [T]) ";
                    _sql += $"select * from [T2], [T] order by [{gridSetting.SortColumn}] {gridSetting.SortOrder.ToString()} offset({gridSetting.PageIndex} - 1) * {gridSetting.PageSize} rows fetch next {gridSetting.PageSize} rows only ";
                }
            }

            SqlCommand objCmd = new SqlCommand(_sql, objConn);

            if (sqlParams != null)
            {
                objCmd.Parameters.AddRange(sqlParams);
            }

            SqlDataAdapter objDa = new SqlDataAdapter(objCmd);
            objDa.Fill(objTab);

            totalCount = (objTab.Rows.Count > 0 ? int.Parse(objTab.Rows[0]["TotalCount"].ToString()) : 0);

            return objTab;
        }
    }
}