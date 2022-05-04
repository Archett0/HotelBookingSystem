using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystem.Areas.Identity.Data;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly HotelBookingSystemContext _customerContext;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            HotelBookingSystemContext customerContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _customerContext = customerContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]                                  // 在这里增加字段
            [DataType(DataType.Text)]
            [Display(Name = "名")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "姓")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "邮箱")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} 长度必须在 {2} 位到 {1} 位之间", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            [Compare("Password", ErrorMessage = "确认密码不一致")]
            public string ConfirmPassword { get; set; }

            public bool IsAdmin { get; set; }    // 是否为管理员,0=customer,1=admin

            public bool IsSubscribedToNewsLetter { get; set; }  // 是否订阅推销邮件

            [Required]
            [Display(Name = "会员类型")]
            public byte MembershipTypeId { get; set; }  //会员类型Id

            [Required]
            [Display(Name = "生日")]
            public DateTime Birthday { get; set; } // 顾客的生日
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)  // 已经登录的用户会被引导至首页
            {
                Response.Redirect("/Home");
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser  // 在这里添加字段
                {
                    UserName = Input.Email, 
                    Email = Input.Email, 
                    FirstName = Input.FirstName, 
                    LastName = Input.LastName,
                    IsAdmin = false,
                    IsSubscribedToNewsLetter = Input.IsSubscribedToNewsLetter,
                    MembershipTypeId = Input.MembershipTypeId,
                    Birthday = Input.Birthday
                };

                // 首先先创建一个Customer
                Customer customer = new Customer();
                customer.Name = Input.LastName + Input.FirstName;
                customer.IsSubscribedToNewsLetter = Input.IsSubscribedToNewsLetter;
                customer.MembershipTypeId = Input.MembershipTypeId;
                customer.Birthday = Input.Birthday;
                customer.Email = Input.Email;   // 已经初始化完成这个customer，看下面的注释

                // 然后执行创建用户的操作
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _customerContext.Customer.Add(customer);    // 如果用户成功注册，则添加这个customer
                    await _customerContext.SaveChangesAsync();
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
