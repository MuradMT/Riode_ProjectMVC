using Riode_ProjectMVC.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode_ProjectMVC.Models;

public class Slider:BaseEntity
{
	public string Title { get; set; }
	public string SubTitle { get; set; }
	public bool? IsLeftSide { get; set; }
	public string ImageName { get; set; }
	[NotMapped]
	public IFormFile ImageFile { get; set; }
}
