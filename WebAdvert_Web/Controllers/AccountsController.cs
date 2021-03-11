using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using WebAdvert_Web.Models.Accounts;

namespace WebAdvert_Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly CognitoSignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;
        private readonly IConfiguration _config;

        public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool pool, IConfiguration config)
        {
            _signInManager = signInManager as CognitoSignInManager<CognitoUser>;
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _pool = pool;
            _config = config;
        }

        public IActionResult Signup()
        {
            var model = new SignupModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                CognitoUser user = _pool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this email already exists!");
                    return View(model);
                }

                user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);

                IdentityResult createdUser = await _userManager.CreateAsync(user, model.Password);
                if (createdUser.Succeeded)
                {
                    return RedirectToAction("Confirm");
                }
                else
                {
                    foreach (IdentityError error in createdUser.Errors)
                        ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Confirm(ConfirmModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> ConfirmPost(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                CognitoUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("NoFound", "An user with a given email address was not found.");
                    return View(model);
                }
                IdentityResult result = await _userManager.ConfirmSignUpAsync(user, model.Code, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError(error.Code, error.Description);

                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("LoginError", "Email and password do not match");
            }

            return View("Login", model);
        }

        [HttpGet]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordPost(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                CognitoUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("NoFound", "An user with a given email address was not found.");
                    return View(model);
                }
                await user.ForgotPasswordAsync();

                return RedirectToAction("ResetPassword");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                CognitoUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("NoFound", "An user with a given email address was not found.");
                    return View(model);
                }
                await user.ConfirmForgotPasswordAsync(model.Code, model.NewPassword);

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}
