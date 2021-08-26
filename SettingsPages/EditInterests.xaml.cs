
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
    public partial class EditInterests : ContentPage
    {
        ObservableCollection<Users> myUser = new ObservableCollection<Users>();
		Users _myUser;
        List<Label> LabelList = new List<Label>();
        InterestsButtons Interests = new InterestsButtons();

        public static bool Clicked1 = false;
        public static bool Clicked2 = false;
        public static bool Clicked3 = false;
        public static bool Clicked4 = false;
        public static bool Clicked5 = false;
        public static bool Clicked6 = false;
        bool[] boolList = { Clicked1, Clicked2, Clicked3, Clicked4, Clicked5, Clicked6 };
        string[] mylist = Data.Constants.Interests;
        int RowsCounter = -1;
        int ColumnCounter = 0;
        int anotherCounter = 0;
        public List<Button> TotalbuttonsList = new List<Button>();

        Users current = new Users();
        public EditInterests(Users _myUser)
        {
            InitializeComponent();
            myUser.Add(_myUser);
            //ShowSettingsListView.ItemsSource = myUser;
            //  eventsManager._myUser = _myUser;
            this._myUser = _myUser;
            current = _myUser;
            ThinkIf();
           // Interests.Init(InterestsGrid, Constants.Interests);

            foreach (string interest in mylist)
            {
                Color dark = Color.LightSeaGreen;
                if (_myUser.Interests.Contains(interest))
                {
                    dark = Color.DarkSlateGray;
                    boolList[anotherCounter] = true;
                }
                if (ColumnCounter % 2 == 0)
                    RowsCounter++;
                Button newBtn = new Button
                {
                    ClassId = "",
                    Text = interest,
                    BackgroundColor = dark,
                    TextColor = Color.White,

                    
                    
                };
                newBtn.Clicked += oneClick;
                InterestsGrid.Children.Add(newBtn, ColumnCounter % 2, RowsCounter);
                TotalbuttonsList.Add(newBtn);
                anotherCounter++;
                ColumnCounter++;
            }                   

        }



        public void ThinkIf()
        {
            string hisInterests = _myUser.Interests;
            List<string> names = hisInterests.Split(',').ToList<string>();
           
            
           /* List<Button> ButtonList = new List<Button> { Sports, Studies, Lifestyle, Books, Movies, Cooking };


            foreach (string item in names)
            {

                foreach (Button btn in ButtonList)
                {
                    if (btn.Text == item)
                    {
                        btn.BackgroundColor = Color.CadetBlue;
                    }
                }
            }*/

        }

        public void oneClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int count= 0;
            foreach (string interest in mylist)
            {
                if (btn.Text== interest)
                {
                    if (boolList[count] == false)
                    {
                        boolList[count] = true;
                        btn.BackgroundColor = Color.DarkSlateGray;
                    }
                    else
                    {
                        boolList[count] = false;
                        btn.BackgroundColor = Color.LightSeaGreen;
                    }
                }
                count = count+1;
            }


        }
        public void continue1(object sender, EventArgs args)
        {
            InterestsGrid.Opacity = 0.6;
            InterestsGrid.IsEnabled = true;
            xBox.IsVisible = false;
            xBoxHelp.IsVisible = false;
            saveBtn.Opacity = 1;
            saveBtn.IsEnabled = true;

        }


        public void SaveInterests(object sender, EventArgs args)
        {
            InterestsString.Text = CreateInterestsString();
            if (InterestsString.Text == "")
            {
                InterestsGrid.Opacity = 0.2;
                saveBtn.Opacity = 0.2;
                saveBtn.IsEnabled = false;
                InterestsGrid.IsEnabled = false;
                xBox.IsVisible = true;
                xBoxHelp.IsVisible = true;

            }
            else
            {
                current.Interests = InterestsString.Text;
                checkBox.IsVisible = true;
                SaveToDateBase();
            }
        }

        protected async void SaveToDateBase()
        {
            await DBItemManager.DefaultManager.SaveUserAsync(current);
            Navigation.RemovePage(this);
        }



        public string CreateInterestsString()
        {
            string interests = "";
            string output;
            int count = 0;
            foreach (bool clk in boolList)
            {
                if (clk)
                {
                    if (interests == "")
                    {
                        interests += mylist[count];
                    }
                    else
                    {
                        interests += "," + mylist[count];
                    }
                }
                count = count + 1;
            }
            return interests;
            output = interests.Substring(0, interests.Length - 1);
            return output;
        }

    }
}