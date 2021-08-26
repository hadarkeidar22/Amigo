using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Amigo.Data
{
    class Constants
    {
		public static bool isDev = true;
		public static string ApplicationURL = "https://amigoapp.azurewebsites.net/";
		public static string[] Interests = new string[] {"ספורט","לימודים","בישול","קריאה","איכות חיים","חיי לילה" };
		public static DateTime DummyDate = DateTime.ParseExact("01/01/1000", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
		public static TimeSpan DummyHour = DateTime.ParseExact("00:00", "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;
		public static string PlacesApiKey = Device.RuntimePlatform == Device.iOS ? "AIzaSyAxXF5lAcwOuMAKQqj04JcLbhxAajYEcVo" : "AIzaSyAOAi6fDtasjwPp-9B6I5JjM-j3yfTqqJo";
		public static string[] colleges = new string[] {"אונ' אריאל","אונ' בן-גוריון","אונ' ת\"א", "האונ' העברית","טכניון"};
        public static string[] collegesFields = new string[] { "ביולוגיה", "הנדסה אזרחית", "הנדסה ביו-רפואית", "הנדסת חומרים", "הנדסת חשמל", "הנדסת מכונות", "הנדסת תוכנה", "הנדסת תעו\"נ", "חינוך", "חשבונאות", "כימיה", "כלכלה", "מדעי המוח", "מדעי המחשב", "מנהל עסקים", "משפטים", "מתמטיקה", "ניהול", "סטטיסטיקה", "פיזיקה", "פילוסופיה", "פסיכולוגיה", "עבודה סוציאלית", "רפואה", "תקשורת"};
        public static int numberOfColleges = 3;
        public static int numberOfCollegesFields = 3;
        public static string[] Dates = new string[] { "Today", "Tomorrow", "this Week", "Next Week"};
        public static string[] imagesLinks = new string[] 
        {
			
        "alcohol.png",
        "basketball.png",
        "beach.png",
        "bicycle.png",
        "dance.png",
        "football.png",
        "gymnastic.png",
        "learningWoman.png",
        "poker.png",
        "readingMan.png",
        "run.png",
        "telescope.png",
        "toast.png",
        "volleyball.png",
        "yoga.png"		
        };
    }
}
