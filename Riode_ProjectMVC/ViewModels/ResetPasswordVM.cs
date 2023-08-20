namespace Riode_ProjectMVC.ViewModels;

public record ResetPasswordVM
{
	[Required, DataType(DataType.Password)]
	public string NewPassword { get; set; }
	[Required, DataType(DataType.Password)]
	public string ConfirmPassword { get; set; }
}
