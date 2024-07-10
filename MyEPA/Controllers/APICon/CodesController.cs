using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.APICon
{
    public class CodesController : Controller
    {
        // GET: Codes
        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// 取得資源調度(類別)
        /// </summary>
        /// <param name="GSLCode"></param>
        /// <returns></returns>
        public ActionResult GetRecTypeItems(int typeItems)
        {
            List<TypeItems> result = new List<TypeItems>();

            try
            {
                //3選1
                if (typeItems == 1)
                {
                    VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
                    result = VehicleTypeRepository.GetList().Select(a => new TypeItems
                                {
                                    Type = a.Type,
                                    Name = a.Name,
                                }).ToList();
                }
                else if (typeItems == 2)
                {
                    DisinfectorTypeRepository DisinfectorTypeRepository = new DisinfectorTypeRepository();
                    result = DisinfectorTypeRepository.GetList().Select(a => new TypeItems
                                {
                                    Type = a.Type,
                                    Name = a.Name,
                                }).ToList();
                }
                else if (typeItems == 3)
                {
                    DisinfectantTypeRepository DisinfectantTypeRepository = new DisinfectantTypeRepository();
                    result = DisinfectantTypeRepository.GetList().Select(a => new TypeItems
                                {
                                    Type = a.Type,
                                    Name = a.Name,
                                }).ToList();
                }

                result = result.OrderBy(a => a.Type).ToList();
                return Json(new { success = true, items = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "執行錯誤：" + ex.Message }, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}