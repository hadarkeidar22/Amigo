using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Amigo.Views.Templates
{
	public class MyDataTemplateSelector : Xamarin.Forms.DataTemplateSelector
	{
		public MyDataTemplateSelector()
		{
			// Retain instances!
			this.incomingDataTemplate = new DataTemplate(typeof(TheirChatMessageTemplate));
			this.outgoingDataTemplate = new DataTemplate(typeof(MyChatMessageTemplate));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			//var context = container.BindingContext;
			//var vm = context as ChatPageViewModel;
			var message = item as Amigo.Models.Messages;
			if (message == null)
				return null;
			return message.Sender != LoadUserDetails.CurrentUser.AuthenticationID ? this.incomingDataTemplate : this.outgoingDataTemplate;
			//Change TO ID BASED!!!
			//return message.Sender != LoadUserDetails.CurrentUser.FirstName + " " + LoadUserDetails.CurrentUser.LastName ? this.incomingDataTemplate : this.outgoingDataTemplate;

		}

		private readonly DataTemplate incomingDataTemplate;
		private readonly DataTemplate outgoingDataTemplate;
	}
	
}
