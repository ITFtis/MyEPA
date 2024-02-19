using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class NewsService
    {
        NewsRepository NewsRepository { get; } = new NewsRepository();

        public List<NewsModel> GetAll()
        {
            return NewsRepository.GetList();
        }

        public NewsModel Get(int id)
        {
            return NewsRepository.Get(id);
        }

        public List<NewsModel> GetByDiasterId(int diasterId)
        {
            return NewsRepository.GetByFilter(new NewsFilterParameter 
            { 
                DiasterIds = diasterId.ToListCollection()
            });
        }

        public void Create(UserBriefModel user, NewsModel model)
        {
            NewsRepository.Create(user, model);
        }

        public List<NewsModel> GetByTop(int top)
        {
            return NewsRepository.GetPageing(new PaginationModel 
            { 
                Order = SortDirectionEnum.DESC,
                Page = 1,
                PerPage = 5,
                SortBy = nameof(NewsModel.Id)
            }).Items;
        }

        public void Update(UserBriefModel user, NewsModel model)
        {
            var entity = NewsRepository.Get(model.Id);
            if(entity == null)
            {
                return;
            }
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.DiasterId = model.DiasterId;

            NewsRepository.Update(user, entity);
        }

        public AdminResultModel Delete(int id)
        {
            var isSuccess = NewsRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = isSuccess
            };
        }

        public List<NewsModel> GetByFilter(NewsFilterParameter filter)
        {
            return NewsRepository.GetByFilter(filter);
        }
    }
}