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
	public partial class CreateEventIntro : ContentPage
	{

		public CreateEventIntro ()
		{
            InitializeComponent();
            
            CreateEventView.Content = new Views.CreateEventView();
            explanation.Text = "\"אמיגו טוב הוא אמיגו שטוב לו\"" +
                "\n" +
                "\n" +
                "ככל שהאמיגו יותר מפורט כך רק האנשים" +
                "\n" +
                "שבכוונתך להזמין יהיו חסופים לו" +
                "\n" +
                "אז יאללה אמיגו!";
        }

        public void goTo1(object sender, EventArgs args)
        {
            header.IsVisible = false;
            continue1.IsVisible = false;
            continue2.IsVisible = true;
            explanation.IsVisible = true;
        }

        public void  goToAddEvent(object sender, EventArgs args)
        {
            continue2.IsVisible = false;
            explanation.IsVisible = false;
            theScrollView.IsEnabled = true;
            CreateEventView.IsVisible = true;
            headerBox.IsVisible = true;
            theScrollView.ScrollToAsync(0, 200, false);
            theScrollView.ScrollToAsync(0, 0, true);
            

        }
		/*
        public void stay(object sender, EventArgs args)
        {
            continue2.IsVisible = false;
            explanation.IsVisible = false;
            Navigation.PushAsync(new CreateNewEvent());
            
        }
        public void notNow(object sender, EventArgs args)
        {
            Navigation.RemovePage(this);
        }
		*/
    }
}