using MyEPA.Models;
using MyEPA.Repositories;

namespace MyEPA.Services
{
    public class ContactManualDownloadRecordService
    {
        ContactManualDownloadRecordRepository ContactManualDownloadRecordRepository = new ContactManualDownloadRecordRepository();
        public PagingResult<ContactManualDownloadRecordModel> GetPagingList(PaginationModel pagination)
        {
            if (pagination == null)
            {
                pagination = new PaginationModel();
            }
            pagination.SortBy = nameof(ContactManualDownloadRecordModel.Id);
            return ContactManualDownloadRecordRepository.GetPageing(pagination);
        }
    }
}