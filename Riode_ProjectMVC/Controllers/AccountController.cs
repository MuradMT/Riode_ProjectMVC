using Riode_ProjectMVC.Services.Interfaces;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Riode_ProjectMVC.Controllers;
public class AccountController : Controller
{
	
	public IAccountService _service { get; }

	public AccountController(IAccountService service)
	{
		_service=service;
	}
	public IActionResult Register()
	{
		if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

		return View();
	}
	[HttpPost]
	[AutoValidateAntiforgeryToken]
	public async Task<IActionResult> Register(RegisterVM register)
	{
		if (!ModelState.IsValid) return View();
		var result = await _service.RegisterAsync(register);
		IdentityResult res = (IdentityResult)result[2];
        AppUser user = (AppUser)result[1];
        string token = (string)result[0];
        if (res.Succeeded)
		{
			var confirmLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, HttpContext.Request.Scheme);
			string body = "Thanks for join our family</br>" + $"please <a href='{confirmLink}'>confirm</a> your account";
			EmailExtension emailHelper = new();
			emailHelper.SendEmail(user.Email, body, "Confirm Your Email");
		}
		else
		{
			foreach (var error in res.Errors)
			{
				ModelState.AddModelError("", error.Description);
				return View();
			}
		}

		return RedirectToAction("Login");
	}
	public IActionResult Login()
	{
		if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
		return View();
	}
	[HttpPost]
	[AutoValidateAntiforgeryToken]
	public async Task<IActionResult> Login(LoginVM loginVM)
	{
		if (!ModelState.IsValid) return View();
		var data=await _service.LoginAsync(loginVM);
		AppUser user = (AppUser)data[0];
		if (user == null)
		{
			ModelState.AddModelError("", "Username or password is wrong");
			return View();
		}
		if (user.EmailConfirmed is false)
		{
			ModelState.AddModelError("", "please, Confirm Your Email");
			return View();
		}
		SignInResult result = (SignInResult)data[1];
		if (!result.Succeeded)
		{
			if (result.IsLockedOut)
			{
				ModelState.AddModelError("", "You trying many times. Please wait about" + $"{user.LockoutEnd} minutes");
				return View();
			}
			else
			{
				ModelState.AddModelError("", "Username or password is wrong");
				return View();
			}
		}
		return RedirectToAction("Index", "Home");
	}
	public async Task<IActionResult> LogOut()
	{
		await _service.LogoutAsync();
		return RedirectToAction("Index", "Home");

	}

	//send confirm email
	public async Task SendConfirmEmail(AppUser user)
	{
		var token = await _service.SendConfirmEmailAsync(user);
		var confirmLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, HttpContext.Request.Scheme);
        EmailExtension emailHelper = new();
		emailHelper.SendEmail(user.Email, confirmLink, "Confirm Your Email");
	}
	public async Task<IActionResult> ConfirmEmail(string token, string email)
	{
		var user = await _service.FindByEmailAsync(email);
		if (user is null) return NotFound();
		await _service.SignInAsync(user, true);
		await _service.ConfirmEmailAsync(user, token);
		return View();
	}
	public IActionResult ForgotPass()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> ForgotPass(ForgotPassVM forgotPassVM)
	{
		if (!ModelState.IsValid) return View();
		var user = await _service.FindByNameAsync(forgotPassVM);

		if (user is null)
		{
			ModelState.AddModelError("UserName", "This user can't be found");
			return View();
		}
		ResetPasswordCode passwordToken = new ResetPasswordCode(user.UserName);
		await _service.ResetPasswordCodesAddAsync(passwordToken);
		var emailcontent = "Reset your password with this link: " + passwordToken.Code;
        EmailExtension email = new();
		email.SendEmail(user.Email, emailcontent, "Reset Your Password");
		return RedirectToAction("ConfirmPassword", "Account");
	}
	public IActionResult ConfirmPassword()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> ConfirmPassword(string resetToken)
	{
		if (String.IsNullOrWhiteSpace(resetToken))
		{
			ModelState.AddModelError("resetToken", "Write the confirm code");
			return View();
		}
		var data= await _service.ConfirmPasswordAsync(resetToken);
		ResetPasswordCode resetPassword = (ResetPasswordCode)data[0];
		if (resetPassword is null)
		{
			ModelState.AddModelError("resetToken", "code is not correct");
			return View();
		}
		AppUser user = (AppUser)data[1];
		await _service.SignInAsync(user, true);
		return RedirectToAction("ResetPassword", "Account");
	}
	public IActionResult ResetPassword()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> ResetPassword(ResetPasswordVM reset)
	{
		if (!ModelState.IsValid) return View();
        var username = User.Identity.Name;
		await _service.ResetPasswordAsync(reset, username);
        return RedirectToAction("Login", "Account");
	}
}