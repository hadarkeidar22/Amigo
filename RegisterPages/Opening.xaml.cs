using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo.RegisterPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Opening : ContentPage
	{
        //List<ContentView> CarouseleViews = new List<ContentView>();

        public Opening ()
		{
			InitializeComponent ();         
        }
		public void AddCollege(object sender, EventArgs args)
		{
			MessagingCenter.Send<Opening>(this, "Details");
		}
	}
}