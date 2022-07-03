using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace PetShop.Controllers
{
    public class HomeController : Controller
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public ActionResult GoOne()
        {
            TempData["Choice"] = "One";
            Session["Choice"] = "One";
            return RedirectToAction("LoginRegister");
        }
        public ActionResult GoTwo()
        {
            TempData["Choice"] = "Two";
            Session["Choice"] = "Two";
            return RedirectToAction("LoginRegister");
        }
        public ActionResult LeaveHome()
        {
            TempData["Choice"] = "One";
            return RedirectToAction("LoginRegister");
        }
        public string FindUser(string user)
        {
            string Result;
            try
            {
                X.Open();
                String G = "Select * from [Member] where Account='"+user+"'";
                SqlCommand Q = new SqlCommand(G, X);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                if (R.Read() == true)
                {
                    Result = R["Password"].ToString().Trim();
                }
                else
                {
                    Result = "非會員";
                }
            }
            catch (Exception)
            {
                Result = "開檔失敗";
            }
            finally { X.Close(); }
            return Result;
        }
        public ActionResult CheckIn()
        {
            string User = Request["UserName"];
            string Pwd = Request["Password"];
            string Ans;
            string CorrectPwd = FindUser(User.Trim());
            if (CorrectPwd=="非會員")
            {
                Ans = "查無此人";
            }
            else
            {
                if (CorrectPwd !=Pwd)
                {
                    Ans = "密碼錯誤";
                }
                else
                {
                    
                    ViewBag.Account = User;
                    Session["LoginUser"] = User;
                    return View("~/Views/Home/Index.cshtml");
                }
            }
            ViewBag.Msg = Ans;
            TempData["Choice"] = "One";
            return View("~/Views/Home/LoginRegister.cshtml");
        }
        public ActionResult LoginRegister()
        {
            if (Session["Choice"] != null)
            {
                TempData["Choice"] = Session["Choice"].ToString();
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["LoginUser"] = null;
            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult Index()
        {
            ViewBag.Account = Session["LoginUser"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}