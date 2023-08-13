﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Riode_ProjectMVC.Models
{
	public class AppUser:IdentityUser
	{
		public string FirstName { get; set; }
		public ICollection<Review> Reviews { get; set; }
		public ICollection<UserBasket> UserBaskets { get; set; }
	}
}
