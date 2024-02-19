using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using MyEPA.Repositories.BaseRepositories;

namespace MyEPA.Models
{
    public class FileUploadRepository : BaseEMISRepository<FileUploadModel>
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
         
        public string ID { get; set; }
        public string FILENAME { get; set; }
        public string FILETEXT { get; set; }
        public string DOWNADMIN { get; set; }
        public string UPTIME { get; set; } 
        public string msg { get; set; }

        public String Add(string FILENAME, string FILETEXT, string DOWNADMIN, string USERLV, string USERID)
        {
            try
            {
                X.Open();
                string G = "INSERT INTO [FileUpload] (UPTIME,FILENAME,FILETEXT,DOWNADMIN,USERLV,USERID) "; 
                G += "VALUES ('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',N'" + FILENAME + "',N'" + FILETEXT + "','" + DOWNADMIN + "'," + "0" + ",'" + USERID + "')";

                SqlCommand Q = new SqlCommand(G, X);
                Q.ExecuteNonQuery();
                X.Close();
                System.Diagnostics.Debug.WriteLine("已新增資料");
                return ("已上傳檔案");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                X.Close();
                return ("上傳檔案失敗");
            }
        }
        public String Delete(string ID)
        {
            try
            {
                X.Open();
                string G = "DELETE FROM[dbo].[FileUpload] WHERE ID = " + ID ;

                SqlCommand Q = new SqlCommand(G, X);
                Q.ExecuteNonQuery();
                X.Close();
                System.Diagnostics.Debug.WriteLine("已刪除檔案");
                return ("已刪除檔案");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                X.Close();
                return ("刪除檔案失敗");
            }
        }

        public LinkedList<FileUploadRepository> Show()
        {
            FileUploadRepository A = new FileUploadRepository();
            LinkedList<FileUploadRepository> C = new LinkedList<FileUploadRepository>();
            string G;
            try
            {
                X.Open(); 
                G = "SELECT *,CASE WHEN [DOWNADMIN]='meup' THEN '同級連同上級' WHEN [DOWNADMIN]='all' THEN '全部層級' WHEN [DOWNADMIN]='same' THEN '同級' WHEN [DOWNADMIN]='myself' THEN '自己' END AS [DOWNADMINTEXT] FROM [FileUpload] ORDER BY UPTIME DESC";
                //G = "SELECT *,CASE WHEN [DOWNADMIN]='meup' THEN '同級連同上級' WHEN [DOWNADMIN]='all' THEN '全部層級' WHEN [DOWNADMIN]='same' THEN '同級' WHEN [DOWNADMIN]='myself' THEN '自己' END AS [DOWNADMINTEXT] FROM [FileUpload] WHERE [USERID]='" + Session["USERNAME"].ToString() + "' " + "ORDER BY UPTIME DESC";
                SqlCommand Q = new SqlCommand(G, X); 
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.ID = Convert.ToString(R["ID"]);
                    A.UPTIME = Convert.ToString(R["UPTIME"]);
                    A.FILENAME = Convert.ToString(R["FILENAME"]);
                    A.FILETEXT = Convert.ToString(R["FILETEXT"]);
                    A.DOWNADMIN = Convert.ToString(R["DOWNADMIN"]); 
                    C.AddLast(A);
                    A = null;
                    A = new FileUploadRepository();
                }
            }
            catch (Exception ex)
            {
                 A.msg = "連通失敗：(" + ex.Message + ")";
                C.AddFirst(A); 
            }
            finally { X.Close(); }
            return C;
        }
    }
}