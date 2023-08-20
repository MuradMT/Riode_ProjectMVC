using Riode_ProjectMVC.Services.Implements;
using Riode_ProjectMVC.Services.Interfaces;
using System.Security.Policy;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Riode_ProjectMVC.Services.Implements;

public class AccountService:IAccountService
{
    public UserManager<AppUser> UserManager { get; }
    public SignInManager<AppUser> SignInManager { get; }
    public RiodeContext RiodeContext { get; }
    public AccountService(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RiodeContext riodeContext)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        RiodeContext = riodeContext;
    }

   
    public async Task<object[]> RegisterAsync(RegisterVM register)
    {
        AppUser user = new()
        {
            FirstName = register.FirstName,
            UserName = register.Username,
            Email = register.Email,

        };
        IdentityResult result = await UserManager.CreateAsync(user, register.Password);
        var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        return new object[3] { token,user,result };
    }

    public async Task<object[]> LoginAsync(LoginVM loginVM)
    {
        AppUser user = await UserManager.FindByEmailAsync(loginVM.UserName)?? await UserManager.FindByNameAsync(loginVM.UserName);
        SignInResult result = await SignInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
        return new object[] { user,result };
    }

    public async Task LogoutAsync()
    {
        await SignInManager.SignOutAsync();
    }
    public async Task<string> SendConfirmEmailAsync(AppUser user)
    {
        return  await UserManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task ConfirmEmailAsync(AppUser user, string token)
    {
         await UserManager.ConfirmEmailAsync(user,token);
    }

    public async Task SignInAsync(AppUser user, bool v)
    {
        await SignInManager.SignInAsync(user, true);
    }

    public async Task<AppUser> FindByEmailAsync(string email)
    {
        return await UserManager.FindByEmailAsync(email);
    }

    public async Task<AppUser> FindByNameAsync(ForgotPassVM forgotPassVM)
    {
        return await UserManager.FindByNameAsync(forgotPassVM.UserName);
    }

    public async Task ResetPasswordCodesAddAsync(ResetPasswordCode token)
    {
        await RiodeContext.ResetPasswordCodes.AddAsync(token);
        await RiodeContext.SaveChangesAsync();
    }

    public async Task<object[]> ConfirmPasswordAsync(string resetToken)
    {
        var resetPassword = RiodeContext.ResetPasswordCodes.Where(r => r.Code == resetToken && r.ExpireTime > TimeSpan.Zero).FirstOrDefault();
        var user = RiodeContext.Users.Where(u => u.UserName == resetPassword.UserName).SingleOrDefault();
        return new object[2] { resetPassword, user };

    }

    public async Task ResetPasswordAsync(ResetPasswordVM reset,string username)
    {
        var user = await UserManager.FindByNameAsync(username);
        var token = await UserManager.GeneratePasswordResetTokenAsync(user);
        await UserManager.ResetPasswordAsync(user, token, reset.NewPassword);
    }
}
