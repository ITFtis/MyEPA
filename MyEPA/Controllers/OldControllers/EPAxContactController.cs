using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class EPAxContactController : LoginBaseController
    {
        ContactModel Contact = new ContactModel();

        //單一聯繫窗口查詢
        public ActionResult A8x4()
        {       
            ViewBag.Data = Contact.Show("ALL");
            return View("~/Views/EPA/A8x4.cshtml");
        }

        //單一聯繫窗口編輯
        public ActionResult A8x5()
        {
            ViewBag.Data = Contact.Show("環境部");
            ViewBag.Msg = TempData["Msg"];
            TempData["Msg"] = null;
            return View("~/Views/EPA/A8x5.cshtml");
        }

        //單一聯繫窗口編輯後更新資料
        public ActionResult Update()
        {
            //要更新的欄位，內容不能為Null，否則更新失敗
            ContactModel Contact = new ContactModel();
            string Place1 = Request["Place1"].ToString().Trim();
            string Line1= Request["Line1"].ToString().Trim();
            string Phone1 = Request["Phone1"].ToString().Trim();
            string Fax1 = Request["Fax1"].ToString().Trim();
            string Mail1 = Request["Mail1"].ToString().Trim();
            string Place2 = Request["Place2"].ToString().Trim();
            string Line2 = Request["Line2"].ToString().Trim();
            string Phone2 = Request["Phone2"].ToString().Trim();
            string Fax2 = Request["Fax2"].ToString().Trim();
            string Mail2 = Request["Mail2"].ToString().Trim();
            string Place3 = Request["Place3"].ToString().Trim();
            string Line3 = Request["Line3"].ToString().Trim();
            string Phone3 = Request["Phone3"].ToString().Trim();
            string Fax3 = Request["Fax3"].ToString().Trim();
            string Mail3 = Request["Mail3"].ToString().Trim();
            string City = "環境部";
            TempData["Msg"] = Contact.Update(Place1, Line1, Phone1, Fax1, Mail1, Place2, Line2, Phone2, Fax2, Mail2, Place3, Line3, Phone3, Fax3, Mail3,City);
            return RedirectToAction("A8x5");
        }
    }
}