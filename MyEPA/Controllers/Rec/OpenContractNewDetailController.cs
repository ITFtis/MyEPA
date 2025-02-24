using System;
using System.Collections.Generic;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewDetailController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();

        // GET: OpenContractNewDetail
        public ActionResult Index(int openContractId)
        {
            ViewBag.OpenContract = OpenContractService.Get(openContractId);

            var result = OpenContractDetailService.GetList(openContractId);

            return View(result);
        }
    }
}