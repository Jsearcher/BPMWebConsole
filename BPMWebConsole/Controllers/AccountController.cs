using AutoMapper;
using BPMWebConsole.Factory.CustomSession;
using BPMWebConsole.Models.ConfigScript;
using BPMWebConsole.Models.Entities;
using BPMWebConsole.Models.Service;
using BPMWebConsole.Models.ViewModels;
using EmailService;
using Lib.Misc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BPMWebConsole.Controllers
{
    /// <summary>
    /// 註冊頁面控制類別
    /// </summary>
    [Route("Account")]
    public class AccountController : Controller
    {
        #region =====[Private] Variable=====

        /// <summary>
        /// Entity與ViewModel間的物件對映功能介面
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 管理所儲存之使用者資料實體的方法API
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// 管理使用者登入的方法API
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Email信件傳送器
        /// </summary>
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Session強型別包裝物件
        /// </summary>
        private readonly ISessionWapper<string> captchaSessionWapper;

        /// <summary>
        /// 登入用圖形驗證碼服務
        /// </summary>
        private readonly CaptchaService captchaService = new CaptchaService();

        #endregion

        #region =====[Public] Constructor & Destructor=====

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="mapper">Entity與ViewModel間的物件對映功能介面</param>
        /// <param name="userManager">管理所儲存之使用者資料實體的方法API</param>
        /// <param name="signInManager">管理使用者登入的方法API</param>
        /// <param name="emailSender">Email信件傳送器</param>
        /// <param name="captchaSessionWapper">圖形驗證碼Session強型別包裝物件</param>
        public AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ISessionWapper<string> captchaSessionWapper)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this.captchaSessionWapper = captchaSessionWapper;
        }

        #endregion

        /// <summary>
        /// 註冊頁面進入點控制
        /// </summary>
        /// <returns>初始註冊頁面</returns>
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// 註冊動作邏輯控制
        /// </summary>
        /// <param name="userModel">註冊頁面模型資料</param>
        /// <returns>
        /// <para>註冊資料驗證成功導向Login頁面</para>
        /// <para>註冊資料驗證失敗重新導入此註冊頁面並顯示錯誤提示</para>
        /// </returns>
        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", userModel);
            }

            User user = this._mapper.Map<User>(userModel);

            IdentityResult result = await this._userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View("Register", userModel);
            }

            await this._userManager.AddToRoleAsync(user, "Guest");

            return RedirectToAction("Login", "Account"); // 有Home Page就改成導向Home Page
        }

        /// <summary>
        /// 忘記密碼頁面進入點控制
        /// </summary>
        /// <returns>初始忘記密碼頁面</returns>
        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassowrd()
        {
            return View("ForgotPassword");
        }

        /// <summary>
        /// 忘記密碼請求動作邏輯控制
        /// </summary>
        /// <param name="forgotPasswordModel">忘記密碼頁面模型資料</param>
        /// <returns>
        /// <para>忘記密碼之Email資料驗證失敗導向ForgotPassword頁面</para>
        /// <para>忘記密碼之Email資料驗證成功並完成發送信件後，導向ForgotPasswordConfirmation頁面</para>
        /// </returns>
        [HttpPost("ForgotPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", forgotPasswordModel);
            }

            User user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation"); // 可能需要其他找不到使用者帳號的確認頁面
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callback = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            Message message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message);

            return View("ForgotPasswordConfirmation");
        }

        /// <summary>
        /// 忘記密碼信件通知之確認頁面進入點控制
        /// </summary>
        /// <returns>初始忘記密碼信件通知確認頁面</returns>
        public IActionResult ForgotPasswordConfirmation()
        {
            return View("ForgotPasswordConfirmation");
        }

        /// <summary>
        /// 依核准的操作代幣初始化密碼重置頁面進入點控制
        /// </summary>
        /// <param name="token">特定使用者有效重置密碼之操作代幣</param>
        /// <param name="email">收件者Email位址</param>
        /// <returns>初始化密碼重置頁面</returns>
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            ResetPasswordModel modelData = new ResetPasswordModel
            {
                Token = token,
                Email = email
            };
            return View("ResetPassword", modelData);
        }

        /// <summary>
        /// 密碼重置請求動作邏輯控制
        /// </summary>
        /// <param name="resetPasswordModel">登入密碼重置操作資料</param>
        /// <returns></returns>
        [HttpPost("ResetPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", resetPasswordModel);
            }

            User user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                RedirectToAction("ResetPasswordConfirmation"); // 可能需要其他找不到使用者帳號的確認頁面
            }

            IdentityResult resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (IdentityError error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View("ResetPassword");
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        /// <summary>
        /// 重置密碼結果確認頁面進入點控制
        /// </summary>
        /// <returns>重置密碼結果確認頁面</returns>
        [HttpGet("ResetPasswordConfirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View("ResetPasswordConfirmation");
        }

        /// <summary>
        /// 進入點頁面控制
        /// </summary>
        /// <returns>初始登入頁面</returns>
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        /// <summary>
        /// 使用者登入作業
        /// </summary>
        /// <param name="modelData">登入頁面資料集合之使用者登入資料</param>
        /// <returns>
        /// <para>驗證成功導向Dashboard頁面</para>
        /// <para>驗證失敗重新導入Login頁面</para>
        /// </returns>
        [HttpPost("UserLogin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelData)
        {
            ModelState.Clear();
            modelData.LoginProp.Account = HttpContext.Request.Form[nameof(LoginViewModel.LoginProp.Account)];
            modelData.LoginProp.Password = HttpContext.Request.Form[nameof(LoginViewModel.LoginProp.Password)];
            modelData.LoginProp.CaptchaString = HttpContext.Request.Form[nameof(LoginViewModel.LoginProp.CaptchaString)];
            ModelState.SetModelValue(string.Join('.', PropertyLib.GetFullPropertyName(typeof(LoginViewModel), nameof(LoginViewModel.LoginProp.Account)).ToArray()),
                new Microsoft.AspNetCore.Mvc.ModelBinding.ValueProviderResult(modelData.LoginProp.Account, System.Globalization.CultureInfo.InvariantCulture));
            ModelState.SetModelValue(string.Join('.', PropertyLib.GetFullPropertyName(typeof(LoginViewModel), nameof(LoginViewModel.LoginProp.Password)).ToArray()),
                new Microsoft.AspNetCore.Mvc.ModelBinding.ValueProviderResult(modelData.LoginProp.Password, System.Globalization.CultureInfo.InvariantCulture));
            ModelState.SetModelValue(string.Join('.', PropertyLib.GetFullPropertyName(typeof(LoginViewModel), nameof(LoginViewModel.LoginProp.CaptchaString)).ToArray()),
                new Microsoft.AspNetCore.Mvc.ModelBinding.ValueProviderResult(modelData.LoginProp.CaptchaString, System.Globalization.CultureInfo.InvariantCulture));

            if (await TryUpdateModelAsync(modelData))
            {
                if (ModelState.IsValid)
                {
                    if (!captchaService.ComputeMd5Hash(modelData.LoginProp.CaptchaString).Equals(this.captchaSessionWapper.Model))
                    {
                        ModelState.AddModelError(string.Empty, "無效的驗證碼");
                        return View("Login", modelData);
                    }

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(modelData.LoginProp.Account, modelData.LoginProp.Password, modelData.LoginProp.RememberMe, false);
                    if (result.Succeeded)
                    {
                        HttpContext.Session.Remove(WebConfig.WebPropertySetting.Instance().SessionKey.CaptchaImg);
                        return RedirectToLocal(modelData.ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "無效的帳號或密碼");
                        return View("Login", modelData);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "無效的帳號或密碼");
            return View("Login", modelData);
        }

        /// <summary>
        /// 使用者登出作業
        /// </summary>
        /// <returns>使用者登出後重新導入至登入頁面</returns>
        [HttpPost("Logout"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account"); // 有Home Page就改成導向Home Page
        }

        /// <summary>
        /// 取得隨機圖形驗證碼
        /// </summary>
        /// <returns>預設4個字元之隨機圖形驗證碼</returns>
        [HttpGet("GetRndCaptcha")]
        public IActionResult GetCaptcha()
        {
            string rndText = this.captchaService.GenerateRandomText(WebConfig.WebPropertySetting.Instance().Captcha.Number);
            this.captchaSessionWapper.Model = this.captchaService.ComputeMd5Hash(rndText);
            return File(this.captchaService.GenerateCaptchaImage(rndText), "image/gif");
        }

        #region =====[Private] Function=====

        /// <summary>
        /// 登入後頁面導向控制
        /// </summary>
        /// <param name="returnRul">頁面導向網址</param>
        /// <returns>導向目標相對網址之網頁或首頁</returns>
        /// <remarks>僅導向網站內部網頁，避免開放式重新導向攻擊</remarks>
        private IActionResult RedirectToLocal(string returnRul)
        {
            if (Url.IsLocalUrl(returnRul))
            {
                return Redirect(returnRul);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard"); // 有Home Page就改成導向Home Page
            }
        }

        #endregion
    }
}
