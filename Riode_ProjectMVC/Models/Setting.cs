using Riode_ProjectMVC.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Riode_ProjectMVC.Models;

public class Setting:BaseEntity
{
	[Required]
	public string Key { get; set; }

	[Required]
	public string Value { get; set; }
}
