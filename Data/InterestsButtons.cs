using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Amigo.Data
{
    class InterestsButtons
    {
		public Color BtnColor { get; set; } = Color.AliceBlue;
		public Color ClickedbtnColor { get; set; } = Color.CadetBlue;
		public bool AllowMultipleButtons { get; set; } = false;
		public int SetColumnsNumInRow { get; set; } = 2;
		int Counter { get; set; } = 0;
		public List<Button> buttonsList = new List<Button>();
		public List<Button> TotalbuttonsList = new List<Button>();
		Grid InterestsGrid;
		int RowsCounter = -1;
		int ColumnCounter = 0;

		public void Init(Grid currentGrid, string[] interests)
		{
			InterestsGrid = currentGrid;
			foreach (string interest in interests)
			{
				if (ColumnCounter % SetColumnsNumInRow == 0)
					RowsCounter++;
				Button newBtn = new Button
				{
					ClassId = "",
					Text = interest,
					BackgroundColor = BtnColor,
					TextColor = Color.Black,
				};
				newBtn.Clicked += ChooseInterest;
				InterestsGrid.Children.Add(newBtn, ColumnCounter % SetColumnsNumInRow, RowsCounter);
				TotalbuttonsList.Add(newBtn);
				ColumnCounter++;
			}
		}
		public void ChooseInterest(object sender, EventArgs args)
		{
			Button btn = (Button)sender;
			if(btn.ClassId=="")
			{
				//uncheck prev button
				if(!AllowMultipleButtons && buttonsList.Count > 0)
				{
					Button prevBtn = buttonsList[0];
					prevBtn.ClassId = "";
					prevBtn.BackgroundColor = BtnColor;
					buttonsList.Remove(prevBtn);
				}
				//
				btn.ClassId = "Clicked";
				btn.BackgroundColor = ClickedbtnColor;
				buttonsList.Add(btn);
			}
			else
			{
				buttonsList.Remove(btn);
				btn.ClassId = "";
				btn.BackgroundColor = BtnColor;
			}
		}
		public string CreateInterestsString()
		{
			string interests = "";
			foreach (Button Btn in buttonsList)
			{
				if(Btn.ClassId == "Clicked")
					interests += Btn.Text + ",";
			}
			if (interests == "")
				return interests;
			return interests.Substring(0, interests.Length - 1);
		}


    }
}
