using Amigo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo.RegisterPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserHobbies : ContentPage
	{
		TagsButtons Tags = new TagsButtons();

		public UserHobbies ()
		{
			InitializeComponent ();
		}

		public void HobbiesaddedButtonClicked(object sender2, EventArgs e)
		{
			if (Morehobbiescell.Text == null)
				Morehobbiescell.Text = "";
			if (Morehobbiescell.Text.Trim() != "")
			{
				Tags.addTag(Morehobbiesgrid, Morehobbiescell);
				Morehobbiescell.Text = "";
			}
		}

		public void AddRegister(object sender, EventArgs args)
		{
			HobbiesString.Text = Tags.CreateTagsString();
			MessagingCenter.Send<UserHobbies>(this, "RegisterPage");
		}

	}
}