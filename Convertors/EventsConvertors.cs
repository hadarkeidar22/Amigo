using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using static Amigo.Data.DBItemManager;
using Amigo.Data;

namespace Amigo.Convertors
{
	public class DescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string description = (string)value;
			if (description.Length < 30)
				return description;
			return description.Substring(0, 30) + "...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class BoolToString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
				return "true";
			else
				return "false";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class DateConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var NoDate = parameter as Label;
			if (NoDate.Text == "true")
				return "Any Date";
			else
				return (string)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class HourConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var NoHour = parameter as Label;
			if (NoHour.Text == "true")
				return "Any Hour";
			else
				return (string)value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class CardBackgroundConvertor : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string eventId = (string)value;
			if (LoadUserDetails.CurrentUser.MyEvents.Contains(eventId))
				return Color.MintCream;
			return Color.White;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    public class CardImageConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //nameofevent
            List<string> optionsList = new List<string>
                {
                "basketball",
                "football",
                "katan",
                "traveling",
                "volleyball",
                "working",
                "yoga",
                "running"
                };
            List<string> optionsLocations = new List<string> {
            "https://preview.ibb.co/gHKB0K/basketball.png",
            "https://preview.ibb.co/nGHg0K/football.png",
            "https://preview.ibb.co/ffFZLK/katan.png",
            "https://preview.ibb.co/d7yM0K/traveling.png",
            "https://image.ibb.co/eQuGZe/volleyball.png",
            "https://preview.ibb.co/m6NELK/working.png",
            "https://preview.ibb.co/gH18fK/yoga.png",
            "https://image.ibb.co/cXBS7z/running.jpg"
            };

            string title = (string)value;
            string[] words = title.Split(' ');
            foreach (string word in words)
            {
                string lowWord = word.ToLower();
                if (optionsList.IndexOf(lowWord) >=0)
                {
                    return optionsLocations[optionsList.IndexOf(lowWord)];
                }
            }
            return "https://image.ibb.co/dZdJjU/bicycle.png";
            //return "https://preview.ibb.co/n6SQSz/copy.jpg";//Amigo logo
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

   // TagsConvertor

        public class TagsConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hashTags = "";
            string AllTags = (string)value;
            List<string> seperatedTags = AllTags.Split(',').ToList<string>();
            foreach(string tag in seperatedTags)
            {
                hashTags = hashTags + "#" + tag + "  ";
            }
            if (AllTags == "")
            {
                return "";
            }
            return hashTags;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
