using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Web.Configuration;
using MyEPA.Repositories;
using System.ComponentModel;

namespace MyEPA.Models
{
    [Table("Registers")]
    public class Registers
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());

        [AutoKey]
        [Required(ErrorMessage = "帳號不能留白")]
        [StringLength(14, MinimumLength = 1, ErrorMessage = "帳號要1-14個字")]
        public string Id { get; set; }

        [Required(ErrorMessage = "姓名不可留白")]
        [StringLength(14, MinimumLength = 1, ErrorMessage = "姓名要1-14個字")]
        public string Name { get; set; }

        [Required(ErrorMessage = "密碼不能留白")]
        [StringLength(14, MinimumLength = 1, ErrorMessage = "密碼要1-14個字")]
        public string Pwd { get; set; }

        public string VoicePwd { get; set; }

        //下面方法無法用，因為會造成無法選擇，原因以後再探討
        //[Required, Range(1, int.MaxValue, ErrorMessage = "必須勾選")]
        public string Duty { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        public string MobilePhone { get; set; }

        public string HumanType { get; set; }

        public string MainContacter { get; set; }

        public string ReportPriority { get; set; }
        [DisplayName("辦公室電話")]
        public string OfficePhone { get; set; }
        [DisplayName("電子郵件信箱")]
        public string Email { get; set; }

        public String Add(string Id, string Name, string Pwd, string Duty, string City, string Town, string MobilePhone, string HumanType, string MainContacter, string ReportPriority,int PositionId)
        {
            string G;
            string Msg;
            var userRepository = new UsersRepository();
            var found = userRepository.GetByUserName(Id);
            if (found != null)
            {
                Msg = "抱歉，與現有用戶同帳號，請換帳號名稱後申請";
                return Msg;
            }
            else
            {
                try
                {
                    int? dutyId = null;
                    int? cityId = null;
                    int? townId = null;

                    if (!string.IsNullOrWhiteSpace(Duty))
                    {
                        var foundDuty = new DutyRepository().GetByDutyName(Duty);
                        dutyId = foundDuty?.Id;
                    }

                    if (!string.IsNullOrWhiteSpace(City))
                    {
                        var foundCity = new CityRepository().GetByCityName(City);
                        cityId = foundCity?.Id;
                    }

                    if (cityId.HasValue && !string.IsNullOrWhiteSpace(Town))
                    {
                        var foundTown = new TownRepository().GetByTownName(cityId.Value, Town);
                        townId = foundTown?.Id;
                    }


                    X.Open();
                    G = @"Insert into Registers(Id,Name,Pwd,VoicePwd,Duty,City,Town, MobilePhone, HumanType,MainContacter,ReportPriority,CityId,TownId,DutyId,PositionId) 
                                        Values(@Id,@Name,@Pwd,'',@Duty,@City,@Town,@MobilePhone, @HumanType,@MainContacter,@ReportPriority,@CityId,@TownId,@DutyId,@PositionId)";
                    SqlCommand Q = new SqlCommand(G, X);
                    Q.Parameters.AddWithValue("@Id", Id);
                    Q.Parameters.AddWithValue("@Name", Name);
                    Q.Parameters.AddWithValue("@Pwd", Pwd);
                    Q.Parameters.AddWithValue("@Duty", Duty);
                    Q.Parameters.AddWithValue("@City", City);
                    Q.Parameters.AddWithValue("@Town", Town);
                    Q.Parameters.AddWithValue("@MobilePhone", MobilePhone);
                    Q.Parameters.AddWithValue("@HumanType", HumanType);
                    Q.Parameters.AddWithValue("@MainContacter", MainContacter);
                    Q.Parameters.AddWithValue("@ReportPriority", ReportPriority);
                    Q.Parameters.AddWithValue("@CityId", cityId);
                    Q.Parameters.AddWithValue("@TownId", townId);
                    Q.Parameters.AddWithValue("@DutyId", dutyId);
                    Q.Parameters.AddWithValue("@PositionId", PositionId);
                    
                    Q.ExecuteNonQuery();
                    X.Close();
                    Msg = Id + "已登錄成功，請等待審核";
                    return (Msg);
                }
                catch (Exception ex)
                {
                    if (X.State == System.Data.ConnectionState.Open)
                    {
                        Msg = "抱歉，申請未成功。資料不完全或是帳號與其它申請者重複。";
                    }
                    //此處只有檢查與Registers是否重複，還沒檢查與User是否重複
                    else { Msg = "不好意思，因為網路未連線，所以無法為您登錄資料，請連接網路後重新登錄"; }
                    return (Msg); //開檔失敗  
                }
                finally { X.Close(); }
            }
        }

        public Registers FindInfo(string id)
        {
            string G = "";
            Registers Person = new Registers();
            Person.Id = "Null"; Person.Name = "Null"; Person.Pwd = "Null"; Person.Duty = "Null"; Person.City = "Null";
            Person.HumanType = "Null"; Person.MainContacter = "Null"; Person.ReportPriority = "Null";
            try
            {
                X.Open();
                G = "Select * from [Registers] where Id= @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", id);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                if (R.Read() == true)
                {
                    Person.Id = id;
                    Person.Name = Convert.ToString(R["Name"]);
                    Person.Pwd = Convert.ToString(R["Pwd"]);
                    Person.Duty = Convert.ToString(R["Duty"]);
                    Person.City = Convert.ToString(R["City"]);
                    Person.Town = Convert.ToString(R["Town"]);
                    Person.HumanType = Convert.ToString(R["HumanType"]);
                    Person.MainContacter = Convert.ToString(R["MainContacter"]);
                    Person.ReportPriority = Convert.ToString(R["ReportPriority"]);
                    return Person;
                }
                else
                {
                    return Person;
                }
            }
            catch (Exception ex)
            {
                return Person;
            }
            finally
            {
                X.Close();
            }
        }

        public String Remove(string id)
        {
            string G;
            string Msg;
            try
            {
                X.Open();
                G = "Delete Registers where Id= @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", id);
                Q.ExecuteNonQuery();
                X.Close();
                Msg = id + "已移出審核名單";
                return (Msg);
            }
            catch (Exception)
            {
                Msg = "抱歉，尚未審核（網路未連線或其它原因)";
                return (Msg);
            }
            finally { X.Close(); }
        }
    }
}