using MyEPA.Repository;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories.BaseRepositories
{
    public class BaseEMISRepository<T> : BaseRepository<T>, IBaseEMISRepository<T>
        where T : class,new()
    {
        public static new string _tableName = typeof(T).Name.Replace("Model", string.Empty);
        private static string connectionString = 
            ConfigurationManager.ConnectionStrings["MyData"]?.ConnectionString;
        public BaseEMISRepository() : base(connectionString, _tableName)
        {

        }
    }
    public class BaseEMISRepository : BaseRepository
    {
        private static string connectionString =
            ConfigurationManager.ConnectionStrings["MyData"]?.ConnectionString;
        public BaseEMISRepository() : base(connectionString)
        {

        }
    }
}