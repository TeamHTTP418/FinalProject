using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using WebMatrix.WebData;
using ReadyPlayerSite.ViewModels;
using DotNetCasClient;
using ReadyPlayerSite.Models;
using System.Text.RegularExpressions;

namespace ReadyPlayerSite.Controllers
{

    public class AccountsController : Controller
    {
        private PlayerDbContext db = new PlayerDbContext();
        //
        // GET: /Account/Login

        [Authorize]
        public ActionResult Login(string returnUrl)
        {
            User user = db.Users.Where(u => u.username == WebSecurity.CurrentUserName).FirstOrDefault();
            if (user == null)
            {
                user = new User { username = WebSecurity.CurrentUserName };
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            else
            {
                Player player = db.Players.Find(user.ID);
                if (player == null)
                {
                    return RedirectToAction("Create");
                }
            }
            return RedirectToAction("Index", "Tasks");
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(bool acceptterms = false)
        {
            if (acceptterms)
            {
                User user = db.Users.Find(WebSecurity.CurrentUserId);
                Player player = new Player { user = user };
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index", "Tasks");
            }
            return View();
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            return RedirectToAction("Login", "Accounts"); //TODO: URL Change
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff(string validation)
        {
            CasAuthentication.SingleSignOut();
            return RedirectToAction("Index", "Tasks"); //TODO: URL Change
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Courses"); //TODO: URL Change
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        /*
        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }
         */
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
