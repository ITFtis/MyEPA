using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MyEPA.Utility;
using Dapper;
using System.Text;
using MyEPA.Extensions;
using System.Linq.Expressions;

namespace MyEPA.Repository
{
    public class BaseRepository
    {
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string _connectionString { get; set; }
        protected void ExecuteSQL(string sql, object pairs)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Execute(sql, pairs);
            }
        }
        protected S ExecuteSQL<S>(string sql, object pairs)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return conn.ExecuteScalar<S>(sql, pairs);
            }
        }
        /// <summary>
        /// 依條件查詢第一筆第一個欄位資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_SQLScript"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected S GetScalarBySQLScript<S>(string _SQLScript, object param = null)
        {
            using (SqlConnection _SqlConnection = new SqlConnection(_connectionString))
            {
                return _SqlConnection.ExecuteScalar<S>(_SQLScript, param);
            }
        }

        /// <summary>
        /// 依條件查詢多筆資料(分頁用)
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="_SQLScript"></param>
        /// <param name="currentPage">目前第幾頁</param>
        /// <param name="pageSize">一頁顯示幾筆資料</param>
        /// <param name="_CommandType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected List<S> GetPageingEntities<S>(string _SQLScript, int currentPage, int pageSize, object param = null)
        {
            int startRowNum = (currentPage <= 1) ? 1 : 1 + (currentPage - 1) * pageSize;
            int endRowNum = (startRowNum - 1) + pageSize;
            string paggingSQL = null;
            if (currentPage.Equals(0))
            {
                paggingSQL = @"
                            SELECT
                                *
                            FROM 
                                ( " + _SQLScript + @" ) as pageData 
                            ";
            }
            else
            {
                paggingSQL = @"
                            SELECT
                                *
                            FROM 
                                ( " + _SQLScript + @" ) as pageData
                            WHERE
                                RowNum >= " + startRowNum + @" AND RowNum <= " + endRowNum + @"
                            ";
            }
            return GetEntitiesBySQLScript<S>(paggingSQL, param);
        }
        /// <summary>
        /// 依條件查詢多筆資料
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="_SQLScript"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected List<S> GetEntitiesBySQLScript<S>(string _SQLScript, object param = null)
        {
            using (SqlConnection _SqlConnection = new SqlConnection(_connectionString))
            {
                return _SqlConnection.Query<S>(_SQLScript, param).ToList();
            }
        }
        protected List<S> GetListBySQL<S>(string sql, object param = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
               return conn.Query<S>(sql, param).ToList();
            }
        }
        /// <summary>
        /// 分頁(初次完成自己可以用，若其他SQL要用請測試過)
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="qrySQL"></param>
        /// <param name="pagination"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected PagingResult<S> GetPageingEntitiesBySQLScript<S>(string qrySQL, PaginationModel pagination, object param = null) where S : class
        {
            string[] strArr = qrySQL.Split(new string[] { "FROM", "from", "From" }, StringSplitOptions.None);
            string str1 = strArr[0] + $",(ROW_NUMBER() OVER (ORDER BY {pagination.SortBy} {pagination.Order.ToString()} )) as RowNum";
            StringBuilder sb = new StringBuilder();
            //跳過第一個
            for (int i = 1; i < strArr.Length; i++)
            {
                sb.Append(@"
                            FROM" + strArr[i]);
            }
            string str2 = sb.ToString();
            qrySQL = str1 + str2;
            string countSQL =
                                @"
                                SELECT
	                                COUNT(0) as cnt 
                                FROM (
                                    " + qrySQL +
                                  @") as DataCount
                                ";
            return GetPageingEntitiesBySQLScript<S>(qrySQL, countSQL, pagination.Page, pagination.PerPage, param);
        }
        /// <summary>
        /// 依條件查詢多筆資料
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="_SQLScript"></param>
        /// <param name="currentPage">目前第幾頁</param>
        /// <param name="pageSize">一頁顯示幾筆資料</param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected PagingResult<S> GetPageingEntitiesBySQLScript<S>(string _SQLScript, string _countSQL, int currentPage, int pageSize, object param = null) where S : class
        {
            PagingResult<S> pageResult = new PagingResult<S>();
            var total = GetScalarBySQLScript<int>(_countSQL, param);
            pageResult.Pagination = new PaginationModel
            {
                Page = currentPage,
                PerPage = pageSize,
                Total = total,
                TotalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1
            };
            pageResult.Items = GetPageingEntities<S>(_SQLScript, currentPage, pageSize, param);
            return pageResult;
        }
    }
    public class BaseRepository<T> : BaseRepository where T : class,new()
    {
        public BaseRepository(string connectionString, string tableName) : base(connectionString)
        {
            _tableName = tableName;
        }

        protected string _tableName { get; set; }

        /// <summary>
        /// 取得列表(請斟酌使用、資料量太大可能會造成系統負荷過大)
        /// </summary>
        /// <param name="qs"></param>
        /// <returns></returns>
        public List<T> GetList()
        {
            return GetListByWhereSQL(string.Empty, null);
        }
        
        /// <summary>
        /// 取得單筆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<S>(S id)
        {
            string key = SQLUtility.GetKeyName<T>().First();
            var sql = $@"
                        SELECT * 
                        FROM {_tableName}
                        WHERE {SQLUtility.GetKeyConditionScript<T>()}";

            Dictionary<string, object> pairs = new Dictionary<string, object> { };

            pairs.Add(key, id);

            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Query<T>(sql, pairs).FirstOrDefault();
            }
        }

        /// <summary>
        /// 新增單筆
        /// </summary>
        public bool Create(T model)
        {
            var sql = SQLUtility.GetInsertCommandByIgnoreId<T>(_tableName);

            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Execute(sql, model) == 1;
            }
        }

        public void CreateOrUpdate(T model)
        {
            CreateOrUpdate(new List<T> { model });
        }

        public void CreateOrUpdate(List<T> models)
        {
            var sql = SQLUtility.GetInsertOrUpdateCommandByIgnoreId<T>(_tableName);

            ExecuteSQL(sql, models);
        }

        public S CreateAndResultIdentity<S>(T model)
        {
            var sql = SQLUtility.GetInsertCommandByIgnoreId<T>(_tableName);

            sql = SQLUtility.ConcatReturnIdentityCommand(sql);

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.ExecuteScalar<S>(sql, model);
                return result;
            }
        }
        protected bool IsExistsByWhereSQL(string whereSql, object param = null)
        {
            var keys = SQLUtility.GetKeyName<T>();
            string sql = string.Empty;
            if (keys.IsNotEmpty())
            {
                string key = keys.First();
                sql = $@"SELECT TOP 1 {key}
                            FROM [{_tableName}] WITH(NOLOCK)
                            {whereSql}";
            }
            else
            {
                sql = $@"SELECT TOP 1 *
                            FROM [{_tableName}] WITH(NOLOCK)
                            {whereSql}";
            }
            return IsExistsBySQL(sql, param);
        }
        protected bool IsExistsBySQL(string sql, object param = null)
        {
            string querySQL = $@"
IF EXISTS 
(
	{sql}
)
BEGIN
     SELECT 1
END
ELSE
     SELECT 0
";
            return GetScalarBySQLScript<bool>(querySQL, param);
        }
        /// <summary>
        /// 新增多筆
        /// </summary>
        public bool Create(List<T> models)
        {
            var sql = SQLUtility.GetInsertCommandByIgnoreId<T>(_tableName);

            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Execute(sql, models) == 1;
            }
        }

        public bool Update(T model)
        {
            var sql = SQLUtility.GetUpdateCommand<T>(_tableName);
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Execute(sql, model) == 1;
            }
        }

        public void Update(List<T> models)
        {
            var sql = SQLUtility.GetUpdateCommand<T>(_tableName);
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute(sql, models);
            }
        }
        /// <summary>
        /// 取得List By Foreignkey
        /// </summary>
        /// <param name="applyId"></param>
        public List<T> GetListByForeignkey(int foreignkeyId)
        {
            string whereSQL = $@"Where {SQLUtility.GetSingleForeignkey<T>()} = @foreignkeyId";
            return GetListByWhereSQL(whereSQL, new { foreignkeyId });
        }
        public List<T> GetListByForeignkey<S>(int foreignkeyId, Expression<Func<T, S>> func)
        {
            string whereSQL = $@"Where {GetFuncFieldName(func)} = @foreignkeyId";
            return GetListByWhereSQL(whereSQL, new { foreignkeyId });
        }
        private string GetFuncFieldName<S>(Expression<Func<T, S>> func)
        {
            return func.Body.ToString().Split('.')[1];
        }
        /// <summary>
        /// 刪除資料 By Foreignkey
        /// </summary>
        /// <param name="applyId"></param>
        public bool DeleteByForeignkey(int foreignkeyId)
        {
            string whereSQL = $@"Where {SQLUtility.GetSingleForeignkey<T>()} = @foreignkeyId";
            return DeleteByWhereSQL(whereSQL, new { foreignkeyId });
        }
        /// <summary>
        /// 刪除資料 By Foreignkey
        /// </summary>
        /// <param name="applyId"></param>
        public bool DeleteByForeignkey<S>(int foreignkeyId, Expression<Func<T,S>> func)
        {
            string whereSQL = $@"Where {GetFuncFieldName(func)} = @foreignkeyId";
            return DeleteByWhereSQL(whereSQL, new { foreignkeyId });
        }
        /// <summary>
        /// 刪除單筆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<S>(S id) where S : IComparable
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var sql = SQLUtility.GetDeleteCommand<T>(_tableName);

                Dictionary<string, object> pairs = new Dictionary<string, object> { };

                pairs.Add(SQLUtility.GetKeyName<T>().First(), id);

                return conn.Execute(sql, pairs) > 0;
            }
        }
        public bool Delete<S>(IEnumerable<S> Ids) where S : IComparable
        {
            string whereSQL = $@"Where {SQLUtility.GetKeyName<T>().First()} IN @Ids";
            return DeleteByWhereSQL(whereSQL, new { Ids });
        }
        protected bool DeleteByWhereSQL(string whereSql, object param = null)
        {
            string sql = $@"DELETE
                            FROM [{_tableName}]
                            {whereSql}";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return conn.Execute(sql, param) > 0;
            }
        }

        protected T GetByWhereSQL(string whereSql, object param = null)
        {
            string querySQL = $@"SELECT * 
                                 FROM [{_tableName}] WITH(NOLOCK)
                                 {whereSql}";
            return GetListBySQL<T>(querySQL, param).FirstOrDefault();
        }
        protected int GetCountByWhereSQL(string whereSql, object param = null)
        {
            var sql = $@"SELECT COUNT(1)
                         FROM {_tableName} WITH(NOLOCK)
                         {whereSql}";

            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.QueryFirstOrDefault<int>(sql, param);
            }
        }

        protected List<T> GetListByWhereSQL(string whereSql, object param = null)
        {
            string querySQL = $@"SELECT * 
                                 FROM [{_tableName}] WITH(NOLOCK)
                                 {whereSql}";
            return GetListBySQL<T>(querySQL, param);
        }
        
        protected PagingResult<T> GetPageingEntitiesByWhereSQL(string whereSQL, PaginationModel pagination, object param = null)
        {
            string querySQL = $@"SELECT * 
                                 FROM {_tableName} WITH(NOLOCK)
                                 {whereSQL}";
            return GetPageingEntitiesBySQLScript<T>(querySQL, pagination, param);
        }
        protected PagingResult<T> GetPageingEntitiesBySQLScript(string qrySQL, PaginationModel pagination, object param = null)
        {
            return GetPageingEntitiesBySQLScript<T>(qrySQL, pagination, param);
        }
        public PagingResult<T> GetPageing(PaginationModel pagination)
        {
            string qrySQL = $@"
                            Select *
                            From {_tableName}";
            return GetPageingEntitiesBySQLScript<T>(qrySQL, pagination, null);
        }
    }
}