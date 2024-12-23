using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPASchedule.Models.Epaemis_local
{
    internal class z_AR4_newCarKind
    {
        public int id { get; set; }
        public string updDate { get; set; }
        public string DBID { get; set; }
        public string ZipID { get; set; }
        public string CityName { get; set; }
        public string TownName { get; set; }
        public string DepName { get; set; }
        public string AssetNo { get; set; }
        public string VhlName { get; set; }
        public string VhlCount { get; set; }
        public string VhlKindName { get; set; }
        public string OtherVhlRecRptCarKindID { get; set; }
        public string CarNo { get; set; }
        public string Capacity { get; set; }
        public string HorsePower { get; set; }
        public string UseMemo { get; set; }
        public string BuyYear { get; set; }
        public string IsEpaSpr { get; set; }
        public string CarNow { get; set; }
        public string CanSupportCity { get; set; }
        public string CanSupportEpa { get; set; }
        public string Memo { get; set; }
        public string TWD97_X { get; set; }
        public string TWD97_Y { get; set; }
        public string IsDeleted { get; set; }
        public string DeletedDate { get; set; }
        public DateTime? WriteTime { get; set; }
    }
}
