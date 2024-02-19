using MyEPA.Models;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class PageExtensions
    {
        public static MvcHtmlString PageLink(this HtmlHelper html, PaginationModel pagination)
        {
            string currentPageStr = $"{nameof(PaginationModel.Page)}";
            string perPageStr = $"{nameof(PaginationModel.PerPage)}";
            int currentPage = pagination.Page;
            int pageSize = pagination.PerPage;
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            //總頁數
            var totalPages = pagination.TotalPage;
            //從ViewContext.RouteData.Values取得目前的RouteValueDictionary
            var routeValueDict = new System.Web.Routing.RouteValueDictionary(html.ViewContext.RouteData.Values);
            routeValueDict[perPageStr] = pageSize;
            routeValueDict[currentPageStr] = pagination.Page;

            if (!string.IsNullOrEmpty(queryString[currentPageStr]))
            {
                //與相應的QueryString綁定
                foreach (string key in queryString.Keys)
                {
                    if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    {
                        routeValueDict[key] = queryString[key];
                    }
                }
                int.TryParse(queryString[currentPageStr], out currentPage);
            }
            else
            {
                //取得 ～/Page/{page number} 的頁號參數
                if (routeValueDict.ContainsKey(currentPageStr))
                {
                    int.TryParse(routeValueDict[currentPageStr].ToString(), out currentPage);
                }
            }
            //保留查詢字元到下一頁
            foreach (string key in queryString.Keys)
            {
                routeValueDict[key] = queryString[key];
            }

            var nav = new TagBuilder("nav");
            nav.Attributes.Add("aria-label", "Page navigation example");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var start = 1;
            var end = totalPages;

            for (int i = start; i <= end; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");

                var a = new TagBuilder("a");
                a.AddCssClass("page-link");

                UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);

                routeValueDict[currentPageStr] = i;

                if(i == currentPage)
                {
                    li.AddCssClass("active");
                }

                if(i != currentPage)
                {
                    string href = url.RouteUrl(routeValueDict);

                    a.Attributes.Add("href", href);
                }               

                a.InnerHtml = i.ToString();
                li.InnerHtml = a.ToString();

                ul.InnerHtml += li.ToString();
            }
            if (end != pagination.TotalPage)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");

                var a = new TagBuilder("a");
                a.AddCssClass("page-link");
                a.Attributes.Add("href", html.ActionLink(pagination.TotalPage.ToString(), routeValueDict["Action"].ToString(), routeValueDict).ToString());
                a.InnerHtml = pagination.TotalPage.ToString();
                li.InnerHtml = a.ToString();

                ul.InnerHtml += li.ToString();
            }

            nav.InnerHtml = ul.ToString();
            
            return new MvcHtmlString(nav.ToString());
        }
    }
}