﻿using System;
namespace Mango.Services.AuthAPI.Models.DTOs
{
	public class LoginRequestDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}

