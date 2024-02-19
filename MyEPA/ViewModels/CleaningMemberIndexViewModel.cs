using MyEPA.Enums;
using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.ViewModels
{
    public class HomeIndexViewModel
    {
        public UserBriefModel UserBrief { get; set; }
        public List<DiasterModel> Diasters { get; set; }
        public List<NoticeModel> Notices { get; set; }
        public List<NewsModel> News { get; set; }


        public string RunningDiasterNames
        {
            get
            {
                var running = Diasters.Where(
                                      c => c.Status == NormalActiveStatusEnum.Active.ToInteger()
                                     );
                return string.Join("，", running.Select(c => c.DiasterName));
            }
        }
    }
}