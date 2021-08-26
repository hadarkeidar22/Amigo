using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Amigo.Data
{
	class TagsButtons
	{
		public int SetColumnsNumInRow { get; set; } = 3;
		int Counter { get; set; } = 0;
		List<Button> buttonsList = new List<Button>();
		Grid TagsGrid;
		int RowsCounter = 1;
		int ColumnCounter = 0;
		bool FirstTimeMsg = true;
		Label TapToDeleteMsg = new Label
		{
			Text = "לחיצה על מילה גורמת למחיקתה",
			TextColor = Color.White,
			BackgroundColor = Color.BlueViolet,
			Opacity = 0,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
		};

		public async void addTag(Grid currentGrid, Entry currentEntry)
		{
			TagsGrid = currentGrid;
			if (ColumnCounter % SetColumnsNumInRow == 0)
				RowsCounter++;

			Button newBtn = new Button
			{
				ClassId = ColumnCounter.ToString(),
				StyleId = RowsCounter.ToString(),
				Text = "#" + currentEntry.Text,
				BackgroundColor = Color.LightGray,
				TextColor = Color.Black,
                Opacity = 0.3,
            };
			newBtn.Clicked += RemoveHobby;
			buttonsList.Add(newBtn);
			TagsGrid.Children.Add(newBtn, ColumnCounter % SetColumnsNumInRow, RowsCounter);
			this.ColumnCounter++;
			if (FirstTimeMsg)
			{
				TagsGrid.Children.Add(TapToDeleteMsg, 1, RowsCounter);
				Grid.SetColumnSpan(TapToDeleteMsg, 2);
				await TapToDeleteMsg.FadeTo(1, 800);
				await Task.Delay(TimeSpan.FromSeconds(1));
				await TapToDeleteMsg.FadeTo(0, 800);
				TagsGrid.Children.Remove(TapToDeleteMsg);
				FirstTimeMsg = false;
			}
		}
		public void RemoveHobby(object sender, EventArgs args)
		{
			Button btn = (Button)sender;

			TagsGrid.Children.Remove(btn);
			if (buttonsList.Count == 1)
			{
				RowsCounter = 1;
				ColumnCounter = 0;
			}
			else
			{
				ColumnCounter--;
				if (ColumnCounter % SetColumnsNumInRow == 0)
					RowsCounter--;
			}
			foreach (Button greaterBtn in buttonsList)
			{
				if (Int32.Parse(greaterBtn.ClassId) > Int32.Parse(btn.ClassId))
				{
					if (Int32.Parse(greaterBtn.ClassId) % SetColumnsNumInRow == 0)
					{
						Grid.SetColumn(greaterBtn, SetColumnsNumInRow - 1);
						Grid.SetRow(greaterBtn, Int32.Parse(greaterBtn.StyleId) - 1);
						greaterBtn.ClassId = (Int32.Parse(greaterBtn.ClassId) - 1).ToString();
						greaterBtn.StyleId = (Int32.Parse(greaterBtn.StyleId) - 1).ToString();

					}
					else
					{
						Grid.SetColumn(greaterBtn, (Int32.Parse(greaterBtn.ClassId) - 1) % SetColumnsNumInRow);
						greaterBtn.ClassId = (Int32.Parse(greaterBtn.ClassId) - 1).ToString();
					}
				}
			}
			buttonsList.Remove(btn);
		}
		public string CreateTagsString()
		{
			string hobbies = "";
			foreach (Button greaterBtn in buttonsList)
			{
				hobbies += greaterBtn.Text.Substring(1, greaterBtn.Text.Length - 1) + ",";
			}
			if (hobbies == "")
				return hobbies;
			return hobbies.Substring(0, hobbies.Length - 1);
		}

	}
}
