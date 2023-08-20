namespace Riode_ProjectMVC.Services.Interfaces;

public interface IAccountService
{
    Task<object[]> RegisterAsync(RegisterVM register);
    Task<object[]> LoginAsync(LoginVM loginVM);
    Task LogoutAsync();
    Task<string> SendConfirmEmailAsync(AppUser user);
    Task ConfirmEmailAsync(AppUser user, string token);
    Task SignInAsync(AppUser user, bool v);
    Task<AppUser> FindByEmailAsync(string email);
    Task<AppUser> FindByNameAsync(ForgotPassVM forgotPassVM);
    Task ResetPasswordCodesAddAsync(ResetPasswordCode token);
    Task<object[]> ConfirmPasswordAsync(string resetToken);
    Task ResetPasswordAsync(ResetPasswordVM reset,string username);
}
