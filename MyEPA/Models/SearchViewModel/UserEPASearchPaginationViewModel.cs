using MyEPA.Enums;

namespace MyEPA.Models.SearchViewModel
{
    public class UserSearchPaginationViewModel
    {
        public int? SearchDutyId { get; set; }
        public int? SearchCityId { get; set; }
        public int? SearchTownId { get; set; }
        public string SearchHumanType { get; set; }
        public string SearchMainContacter { get; set; }
    }
    public class UserEPASearchPaginationViewModel
    {
        public UserEPASearchPaginationViewModel()
        {
            Pagination = new PaginationModel();
        }
        public int? SearchDepartmentId { get; set; }
        public int? SearchPositionId { get; set; }
        public int? SearchTownId { get; set; }
        public string SearchName { get; set; }
        public PaginationModel Pagination { get; set; }
    }
    public class UserEPBSearchViewModel
    {
        public int? SearchCityId { get; set; }
    }
    public class UserSearchViewModel
    {
        public int? SearchDepartmentId { get; set; }
        public int? SearchPositionId { get; set; }
        public int? SearchTownId { get; set; }
        public string SearchName { get; set; }
        public DutyEnum? SearchDutyId { get; set; }
    }
    public class UserEPASearchViewModel
    {
        public int? SearchDepartmentId { get; set; }
        public int? SearchPositionId { get; set; }
        public int? SearchTownId { get; set; }
        public string SearchName { get; set; }
        public string SearchDepartmentName { get; set; }
        public ContactManualDutyEnum ContactManualDuty { get; set; }
    }
}