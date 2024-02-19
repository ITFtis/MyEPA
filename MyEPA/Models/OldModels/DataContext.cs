using System.Data.Entity;
namespace MyEPA.Models
{
    //以下是使用EntityFramework的GridView來顯示資料
    //分別用於顯示註冊者，等待刪除的通告、新聞、用戶

    public class DataContext : DbContext
    {
        public DbSet<Registers> Register { get; set; }
        public DbSet<DeleteNoticeModel> DeleteNotice{ get; set; }
        public DbSet<DeleteNewsModel> DeleteNews { get; set; }
        public DbSet<UserModel> User { get; set; }
        
        //公用垃圾掩埋法在舊方法採用GridView去刪除
        //之後若方法變動，以下這行可刪除
        public DbSet<DeleteLandfillModel> DeleteLandfill { get; set; }

        //在環保署用戶進入通報管考時，
        //會以GridView顯示所有縣市的消毒設備、藥品...之數量
        //此顯示採用以下NewestCountValue的GridView

        public DbSet<NewestCountValueModel> NewestCountValue { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.OpenContractModel> OpenContractModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.OpenContractDetailModel> OpenContractDetailModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.MutualSupportModel> MutualSupportModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.ViewModels.MutualSupportViewModel> MutualSupportViewModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.ViewModels.MutualSupportSearchViewModel> MutualSupportSearchViewModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.ViewModels.ShiftScheduleViewModel> ShiftScheduleViewModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.ApplyPeopleModel> ApplyPeopleModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.ViewModels.PhoneWorkViewModel> PhoneWorkViewModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.WaterCheckDetailModel> WaterCheckDetailModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.PolymerDetailModel> PolymerDetailModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.WaterEquipmentModel> WaterEquipmentModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.NoticeModel> NoticeModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.DamageModel> DamageModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.SendTextLogModel> SendTextLogModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.SendTextLogDetailModel> SendTextLogDetailModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.ViewModels.DamageViewModel> DamageViewModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.FileDataModel> FileDataModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.WaterCheckModel> WaterCheckModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.MailModel> MailModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.ContactManualModel> ContactManualModels { get; set; }

        public System.Data.Entity.DbSet<MyEPA.Models.ContactManualDownloadRecordModel> ContactManualDownloadRecordModels { get; set; }
    }
}