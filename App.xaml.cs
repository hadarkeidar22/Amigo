using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Amigo.Data;
using Amigo;
using System.Collections.Generic;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Amigo
{
	public partial class App : Application
	{
        public static string AzureBackendUrl = Constants.ApplicationURL;
		public const string AppName = "Amigo";

		public App ()
		{
			InitializeComponent();
			MainPage = new NavigationPage(new WelcomePage());
           // MainPage = new NavigationPage(new LoadUserDetails("1",""));
           // MainPage = new NavigationPage(new SearchEventPage());
            //MainPage = new NavigationPage(new RegisterPage());
            //MainPage = new NavigationPage(new WelcomePage());
            //MainPage = new NavigationPage(new CreateEventIntro());
        }
		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

	}
}
