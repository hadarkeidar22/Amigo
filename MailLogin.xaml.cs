using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MailLogin : ContentPage
    {
        

        public MailLogin()
		{
			InitializeComponent ();
            header.Text = "הגיע הזמן להתחבר";
           // theImage.FadeTo(0, 6000);
            
        }
        public void AddDetails(object sender, EventArgs args)
        {
            MessagingCenter.Send<MailLogin>(this, "Details");
            logInLbl.Opacity = 0.6;
            logInBox.Opacity = 0.6;
        }
    }
}