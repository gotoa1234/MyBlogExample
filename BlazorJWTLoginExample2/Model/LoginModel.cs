﻿namespace BlazorJWTLoginExample2.Model
{
    public class LoginModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}