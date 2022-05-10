using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingSystem.Areas.Identity.Data;
using HotelBookingSystem.Data;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBookingSystem.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly HotelBookingSystemContext _customerContext;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            HotelBookingSystemContext customerContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerContext = customerContext;
        }

        [Display(Name = "用户名(不可修改)")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "手机号码")]
            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "名")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "姓")]
            public string LastName { get; set; }

            public bool IsSubscribedToNewsLetter { get; set; }

            [Required]
            [Display(Name = "生日")]
            [DataType(DataType.Date)]
            public DateTime Birthday { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var isSubscribedToNewsLetter = user.IsSubscribedToNewsLetter;
            var birthday = user.Birthday;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                IsSubscribedToNewsLetter = isSubscribedToNewsLetter,
                Birthday = birthday,
        };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.IsSubscribedToNewsLetter = Input.IsSubscribedToNewsLetter;
            user.Birthday = Input.Birthday;
            var currentCustomer = _customerContext.Customer.FirstOrDefault(c => c.Email == user.Email);
            if (currentCustomer == null)
            {
                StatusMessage = "更新信息时出现未知错误,请稍后重试";
                return RedirectToPage();
            }
            var setPreferenceResult = await _userManager.UpdateAsync(user);
            if (!setPreferenceResult.Succeeded)
            {
                StatusMessage = "更新信息时出现未知错误,请稍后重试";
                return RedirectToPage();
            }   // edited part end

            // now the User has been updated, we still have to update our Customer form
            currentCustomer.Name = Input.LastName + Input.FirstName;
            currentCustomer.IsSubscribedToNewsLetter = Input.IsSubscribedToNewsLetter;
            currentCustomer.Birthday = Input.Birthday;
            _customerContext.Customer.Update(currentCustomer);  // update
            await _customerContext.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "个人信息更新成功";
            return RedirectToPage();
        }
    }
}
