using System;
using System.Web.Mvc;
using System.Text;
using System.Reflection;
using log4net;
using BEYON.Component.Tools;
using BEYON.CoreBLL.Service;
using BEYON.CoreBLL.Service.Member.Interface;
using BEYON.ViewModel;

namespace BEYON.Web.Areas.Common.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Common/Login/

        public ActionResult Index()
        {
            return View();
        }
        private readonly IAccountService _AccountService;
        public LoginController(IAccountService accountService)
        {
            this._AccountService = accountService;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//防止XSS攻击
        public ActionResult Index(LoginVM loginVM)
        {
            try
            {
                OperationResult result = _AccountService.Login(loginVM);
                string msg = result.Message ?? result.ResultType.GetDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    _log.Info(String.Format("登录用户[ {0} ]", loginVM.LoginName));

                    //  return Redirect(Url.Action("Index", "Home"));
                    //return RedirectToAction("Index", "Home");
                    StringBuilder sb = new StringBuilder("http://");
                    sb.Append(Request.Url.Host);
                    sb.Append(":");
                    sb.Append(Request.Url.Port);
                    sb.Append("/Common/Home#/App/ApplyForm/Index");
      
                    return Redirect(sb.ToString());
                }
                ModelState.AddModelError("", msg);
                return View(loginVM);
            }
            catch (Exception e)
            {
                _log.Fatal(e);

                ModelState.AddModelError("", e);
                return View(loginVM);
            }
        }

        public ActionResult Logout()
        {
            _AccountService.Logout();
            return RedirectToAction("Index", "Home");
        }


    }
}
