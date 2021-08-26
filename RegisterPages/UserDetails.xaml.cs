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
	public partial class UserDetails : ContentPage
	{
		public UserDetails()
		{
			InitializeComponent ();
            Gender.SelectedItem = "מין";

            extraLbl.Text="מידע מהימן עוזר לאמיגו למצוא לך פרטנרים נכונים יותר המותאמים אישית ממש אליך !";

        }
		public void AddCollege(object sender, EventArgs args)
		{
			MessagingCenter.Send<UserDetails>(this, "College");
		}
	}
}