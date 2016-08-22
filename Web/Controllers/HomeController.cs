using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Business_Layer;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ExpenseDAL dal = new ExpenseDAL();
            dal.GetExpenses();
            return View();
        }

        public ActionResult About()
        {
            ExpenseDAL dal = new ExpenseDAL();
            IEnumerable<ExpenseModel> expense = dal.GetExpenses();
            return View("About",expense);
        }

        public ActionResult Expense()
        {          
            return View();
        }


        public JsonResult GetCat()
        {
            ExpenseDAL dal = new ExpenseDAL();
            IEnumerable<CategoryModel> cat = dal.GetCat();
            return Json(cat, JsonRequestBehavior.AllowGet);
        }

 
        public ActionResult SaveExpense(ExpenseModel exp)
        {
            ExpenseDAL dal = new ExpenseDAL();
            IEnumerable<ExpenseModel> expense = dal.SaveExpense(exp);
            return View("About", expense);
        }

        public JsonResult SaveCat(string category_name)
        {
            ExpenseDAL dal = new ExpenseDAL();
            IEnumerable<CategoryModel> cat = dal.SaveCat(category_name);
            return Json(cat, JsonRequestBehavior.AllowGet);
        }
    }
        
}