using MyEPA.Extensions;
using MyEPA.Models.TWMapModels;
using RestSharp;
using System.Net;

namespace MyEPA.Services
{
    public class TWMapService
    {
        public TWMapGPSResultModel Coordinate(string searchWord)
        {
            TWMapGPSFunModel<FunBModel> funBResult = GetFunB(searchWord);
            if (string.IsNullOrWhiteSpace(funBResult?.locate?.landmark) == false)
            {
                return new TWMapGPSResultModel
                {
                    GpsX = funBResult.locate.lat,
                    GpsY = funBResult.locate.lng
                };
            }
            TWMapGPSFunModel<FunAModel> funAResult = GetFunA(searchWord);
            if (funAResult?.locate == null || string.IsNullOrWhiteSpace(funAResult.locate.bname2))
            {
                return new TWMapGPSResultModel(false);
            }
            return new TWMapGPSResultModel
            {
                GpsX = funAResult.locate.lat,
                GpsY = funAResult.locate.lng
            };
        }
        private static TWMapGPSFunModel<FunAModel> GetFunA(string searchWord)
        {
            string url = "https://api.map.com.tw/net/GraphicsXY_TWMAP.aspx";
            var client = new RestClient($"{url}?search_class=address&searchkey=32FAFAA12E07573A06C6BAFFCC206D162C7C9D49&fun=funA&SearchWord={searchWord}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Host", " api.map.com.tw");
            request.AddHeader("Connection", " keep-alive");
            request.AddHeader("Pragma", " no-cache");
            request.AddHeader("Cache-Control", " no-cache");
            request.AddHeader("sec-ch-ua", " \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            request.AddHeader("DNT", " 1");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36";
            request.AddHeader("Accept", " */*");
            request.AddHeader("Sec-Fetch-Site", " same-site");
            request.AddHeader("Sec-Fetch-Mode", " no-cors");
            request.AddHeader("Sec-Fetch-Dest", " script");
            request.AddHeader("Referer", " https://www.map.com.tw/");
            request.AddHeader("Accept-Encoding", " gzip, deflate, br");
            request.AddHeader("Accept-Language", " zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cookie", " ServerName=www%2Emap%2Ecom%2Etw; ASPSESSIONIDAUARBCBA=MEEBGEDAEIMLDMGNHLFDONCD; ASP.NET_SessionId=mvfj55s1sqkg2mdtu42kcp24");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string resp = client.Execute(request).Content;
            int startIndex = resp.IndexOf("{");
            int endIndex = resp.LastIndexOf("}");
            string json = resp.Substring(startIndex, endIndex - startIndex + 1);
            var model = json.JsonConvertToModel<TWMapGPSFunModel<FunAModel>>();
            return model;
        }
        private static TWMapGPSFunModel<FunBModel> GetFunB(string searchWord)
        {
            string url = "https://api.map.com.tw/net/GraphicsXY_TWMAP.aspx";
            var client = new RestClient($"{url}?city=&area=&search_class=Landmark&SearchWord={searchWord}&searchkey=32FAFAA12E07573A06C6BAFFCC206D162C7C9D49&fun=funB");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Host", " api.map.com.tw");
            request.AddHeader("Connection", " keep-alive");
            request.AddHeader("Pragma", " no-cache");
            request.AddHeader("Cache-Control", " no-cache");
            request.AddHeader("sec-ch-ua", " \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            request.AddHeader("DNT", " 1");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36";
            request.AddHeader("Accept", " */*");
            request.AddHeader("Sec-Fetch-Site", " same-site");
            request.AddHeader("Sec-Fetch-Mode", " no-cors");
            request.AddHeader("Sec-Fetch-Dest", " script");
            request.AddHeader("Referer", " https://www.map.com.tw/");
            request.AddHeader("Accept-Encoding", " gzip, deflate, br");
            request.AddHeader("Accept-Language", " zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cookie", " ServerName=www%2Emap%2Ecom%2Etw; ASPSESSIONIDAUARBCBA=MEEBGEDAEIMLDMGNHLFDONCD; ASP.NET_SessionId=mvfj55s1sqkg2mdtu42kcp24");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            string resp = client.Execute(request).Content;

            int startIndex = resp.IndexOf("{");
            int endIndex = resp.LastIndexOf("}");
            string json = resp.Substring(startIndex, endIndex - startIndex + 1);
            var model = json.JsonConvertToModel<TWMapGPSFunModel<FunBModel>>();
            return model;
        }
    }
}