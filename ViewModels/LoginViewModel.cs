using Amigo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Amigo.ViewModels
{
    public class LoginViewModel
    {
        private ApiServices _apiServices = new ApiServices();

        public string Email { get; set; }
        public string Password { get; set; }
		/*
        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var accesstoken  = await _apiServices.LoginAsync(Email, Password);

                    Settings.AccessToken = accesstoken;
                });
            }
        }
        public LoginViewModel()
        {
            Email = Settings.Username;
            Password = Settings.Password;

        } 
		*/
    }
}
