
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.WindowsAzure.MobileServices;
using Amigo.Data;
using System.Diagnostics;
using System.Collections.ObjectModel;
using static Amigo.Data.DBItemManager;
using Amigo.Models;

namespace Amigo.SettingsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditHobbies : ContentPage
    {
		Users current = new Users();
		Users _myUser;
        public EditHobbies(Users _myUser)
        {
            InitializeComponent();

            //ShowSettingsListView.ItemsSource = myUser;
            //eventsManager = EventsItemManager.DefaultManager;
            //  eventsManager._myUser = _myUser;
            this._myUser = _myUser;
            current = _myUser;
            DisplayHistoricHobbies();
            

        }

        TagsButtons Tags = new TagsButtons();

        public void DisplayHistoricHobbies()
        {
            if (current.Hobbies != "")
            {
                string allHobbies = current.Hobbies;
                List<string> seperatedHobbies = allHobbies.Split(',').ToList<string>();
                foreach (string item in seperatedHobbies)
                {
                    unSeen.Text = item;
                    Tags.addTag(Morehobbiesgrid, unSeen);

                }
            }
        }

        void HobbiesaddedButtonClicked(object sender2, EventArgs e)
        {
            if (Morehobbiescell.Text == null)
                Morehobbiescell.Text = "";
            if (Morehobbiescell.Text.Trim() != "")
            {
                Tags.addTag(Morehobbiesgrid, Morehobbiescell);
                Morehobbiescell.Text = "";
            }
        }

        public void saveDataclicked(object sender, EventArgs args)
        {
            HobbiesString2.Text = Tags.CreateTagsString();
            if (HobbiesString2.Text == "")
            {
                xBox.IsVisible = true;
                xBoxHelp.IsVisible = true;
                Morehobbiesgrid.Opacity = 0.4;
                Morehobbiesgrid.IsEnabled = false;
                saveData.Opacity = 0.4;
                saveData.IsEnabled = false;
            }
            else
            {
                HobbiesString.Text = Tags.CreateTagsString();
                current.Hobbies = HobbiesString.Text;
                checkBox.IsVisible = true;
                SaveToDateBase();
            }
        }
        protected async void SaveToDateBase()
        {

            await DBItemManager.DefaultManager.SaveUserAsync(current);
            Navigation.RemovePage(this);
        }
        public void continue1(object sender, EventArgs args)
        {
            xBox.IsVisible = false;
            xBoxHelp.IsVisible = false;
            Morehobbiesgrid.Opacity = 1;
            Morehobbiesgrid.IsEnabled = true;
            saveData.Opacity = 1;
            saveData.IsEnabled = true;

        }




        }
}

