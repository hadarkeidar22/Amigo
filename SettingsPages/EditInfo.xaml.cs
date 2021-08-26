
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
    public partial class EditInfo : ContentPage
    {
        ObservableCollection<Users> myUser = new ObservableCollection<Users>();
		Users _myUser;
        List<Label> LabelList = new List<Label>();
		Users current = new Users();
        string collegePickerHelp = "";
        string collegePickerFieldHelp = "";
        string collegePickerFieldHelp2 = "";
        string[] Colleges = Constants.colleges;
        string[] CollegesFields = Constants.collegesFields;
        int numberOfColleges = Constants.numberOfColleges;
        int numberOfCollegesFields = Constants.numberOfCollegesFields;


        public EditInfo(Users _myUser)
        {
            InitializeComponent();
            myUser.Add(_myUser);
            //ShowSettingsListView.ItemsSource = myUser;
            //eventsManager._myUser = _myUser;
            this._myUser = _myUser;
            current = _myUser;
            DisplayThisPage();
        }

        private void DisplayThisPage()
        {
            FirstName.Text = current.FirstName;
            LastName.Text = current.LastName;
			Birthday.Date = current.Birthday;
            //City.Text = current.Location;
            var collegePicker = this.FindByName<Picker>("College");
            collegePicker.SelectedItem = current.College;
            string hisFields = current.CollegeFields;
            List<string> seperatedFields = hisFields.Split(',').ToList<string>();
            var FirstCollegeFieldPicker = this.FindByName<Picker>("FirstCollegeField");
            FirstCollegeFieldPicker.SelectedItem = seperatedFields[0];
            if (seperatedFields.Count == 2)
            {
                var SecondCollegeFieldPicker = this.FindByName<Picker>("SecondCollegeField");
                SecondCollegeFieldPicker.SelectedItem = seperatedFields[1];
                AddCollegeFieldButton.IsVisible = false;
                SecondCollegeField.IsVisible = true;
                secongFieldLbl.IsVisible = true;
                RemoveCollegeFieldButton.IsVisible = true;
            }
            ClassOf.Text = current.ClassOf.ToString();
        }

        private void AddCollegeField(object sender, EventArgs e)
        {
            (sender as Button).IsVisible = false;
            SecondCollegeField.IsVisible = true;
            RemoveCollegeFieldButton.IsVisible = true;
            secongFieldLbl.IsVisible = true;
        }
        private void RemoveCollegeField(object sender, EventArgs e)
        {
            (sender as Button).IsVisible = false;
            AddCollegeFieldButton.IsVisible = true;
            SecondCollegeField.IsVisible = false;
            secongFieldLbl.IsVisible = false;
        }


		private void SaveChanges(object sender, EventArgs e)
        {
            var collegePicker = this.FindByName<Picker>("College");
            collegePickerHelp = collegePicker.Items[collegePicker.SelectedIndex];
            var FirstCollegeFieldPicker = this.FindByName<Picker>("FirstCollegeField");
            int[] FieldsArray = new int[numberOfCollegesFields];
                collegePickerHelp = collegePicker.Items[collegePicker.SelectedIndex];
                collegePickerFieldHelp = FirstCollegeFieldPicker.Items[FirstCollegeFieldPicker.SelectedIndex];

                var SecondCollegeFieldPicker = this.FindByName<Picker>("SecondCollegeField");
                if (SecondCollegeField.IsVisible == true && CollegesFields.Any(item => item == SecondCollegeFieldPicker.SelectedItem.ToString()))
                {
                    collegePickerFieldHelp2 = SecondCollegeFieldPicker.Items[SecondCollegeFieldPicker.SelectedIndex];
                }

                if (FirstName.Text == "" || LastName.Text == "" || City.Text == "" ||
                    collegePickerHelp == "" || collegePickerFieldHelp == "" || ClassOf.Text == "")
                {
                    DisplayAlert("Oh No", "number one", "let's fix it");
                }
                else
                {
                    current.FirstName = FirstName.Text;
                    current.LastName = LastName.Text;
                    current.Birthday = Birthday.Date;
                    //current.Location = City.Text;
                    current.College = collegePickerHelp;
                    current.ClassOf = Int32.Parse(ClassOf.Text);
                    if (SecondCollegeField.IsVisible == true)
                    {
                        if (collegePickerFieldHelp2 != "")
                        {
                            current.CollegeFields = collegePickerFieldHelp + "," + collegePickerFieldHelp2;
                        }
                        else
                        {
                            current.CollegeFields = collegePickerFieldHelp;
                        }
                    }
                    else
                    {
                        current.CollegeFields = collegePickerFieldHelp;
                    }
                   // DisplayAlert("you did it Amigo", "You Updated your Info", "Yeah");
                    SaveToDateBase();
                checkBox.IsVisible = true;
                

                }

            }

       
        protected async void SaveToDateBase()
        {
            await DBItemManager.DefaultManager.SaveUserAsync(current);
            Navigation.RemovePage(this);
        }
    }
}