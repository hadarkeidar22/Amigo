using Amigo.Models;
using Amigo.ViewModels;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessegesView : ContentView
	{

		public MessegesView(Events _selectedEvent)
		{
			InitializeComponent();
			((ChatPageViewModel)this.BindingContext).Room = _selectedEvent;
		}
	}
}