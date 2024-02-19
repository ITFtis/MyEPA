using MyEPA.Helper;
using System;

namespace MyEPA.Models.BaseModels
{
    public class BaseLoggerModel
    {
        public BaseLoggerModel(object model)
        {
            Model = model;
            Time = DateTimeHelper.GetCurrentTime();
        }
        public object Model { get; set; }
        public DateTime Time { get; set;}
    }
}