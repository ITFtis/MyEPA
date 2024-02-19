using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.TWMapModels
{
    public class TWMapGPSResultModel
    {
        public TWMapGPSResultModel(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }

        public string GpsX { get; set; }
        public string GpsY { get; set; }

        public bool IsSuccess { get; set; }
    }
    public class TWMapGPSFunModel<T>
    {
        public T locate { get; set; }
    }
    public class FunAModel
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string no { get; set; }
        public string bheigh2 { get; set; }
        public string bcount2 { get; set; }
        public string baddr2 { get; set; }
        public string bname2 { get; set; }
        public string village { get; set; }
        public string road { get; set; }
        public string msg { get; set; }
        public string precision { get; set; }
    }
    public class FunBModel
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string landmark { get; set; }
        public string address { get; set; }
    }
}