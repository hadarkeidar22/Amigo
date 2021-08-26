using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Amigo.Models;
namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserProfile : ContentPage
	{
		public UserProfile(Users _currentUser)
		{
			InitializeComponent();
			UserPhoto.Source = UriImageSource.FromUri(new Uri(_currentUser.Image));
			UserFullName.Text = _currentUser.FirstName + " " + _currentUser.LastName;
			UserAge.Text = _currentUser.Age.ToString();
			UserCollegeLabel.Text = _currentUser.College + "(" + _currentUser.CollegeFields + ")";
		}
	}
}