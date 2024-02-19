using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class BreadCrumbService
    {
        private static List<BreadCrumbModel> ConvertToBreadCrumbs(string input)
        {
            return input.Split(',').Select(e => new BreadCrumbModel { Title = e }).ToList();
        }
        static List<BreadCrumbModel> BaseContactManual = ConvertToBreadCrumbs("手冊資料管理,手冊資料維護");
        static List<BreadCrumbModel> BaseSystemContactManual = ConvertToBreadCrumbs("基本資料管理");
        static List<BreadCrumbModel> BaseContactManualDataUpload = ConvertToBreadCrumbs("手冊資料管理,手冊資料上傳");
        Dictionary<ContactManualBreadCrumbTypeEnum, List<BreadCrumbModel>> BreadCrumbs = new Dictionary<ContactManualBreadCrumbTypeEnum, List<BreadCrumbModel>>()
        {
            { ContactManualBreadCrumbTypeEnum.ContactManualEPA, BaseContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualEPB, BaseContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualEPAOther, ConvertToBreadCrumbs("電子手冊資料管理,各業務單位緊急應變通聯表") },
            { ContactManualBreadCrumbTypeEnum.ContactManualEPARole, ConvertToBreadCrumbs("電子手冊資料管理,應變名冊") },
            { ContactManualBreadCrumbTypeEnum.UserEPA, ConvertToBreadCrumbs("系統管理" )},
            { ContactManualBreadCrumbTypeEnum.ContactManualDepartment, BaseSystemContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualRole, BaseSystemContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualSupervise, BaseSystemContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualEPASupervise,ConvertToBreadCrumbs("電子手冊資料管理,春節期間督導責任區域劃分表") },
            { ContactManualBreadCrumbTypeEnum.ContactManualRecycle, BaseContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualOnDuty, BaseContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManual24OnDuty, BaseContactManual },
            { ContactManualBreadCrumbTypeEnum.ContactManualFileData, BaseContactManualDataUpload },
            { ContactManualBreadCrumbTypeEnum.ContactManualSupervisionFileData, BaseContactManualDataUpload },
            
        };
        public List<BreadCrumbModel> GetBreadCrumbsByType(ContactManualBreadCrumbTypeEnum type)
        {
            return BreadCrumbs.GetValue(type, new List<BreadCrumbModel>());
        }
    }
}