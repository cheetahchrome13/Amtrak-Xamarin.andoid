using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Amtrak
{
	[Activity(Label = "SecondActivity")]
	public class SecondActivity : Activity
	{
		string backingFile = MainActivity.backingFile;
		List<int> numbers = new List<int>();
		TextView label;
		TextView departure;
		TextView arrival;
		TextView hidden;
		Button delete;
		ImageView icon;
		bool isRedEye;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="savedInstanceState"></param>
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.activity_second);
			//label = FindViewById<TextView>(Resource.Id.arrivalLabel);
			departure = FindViewById<TextView>(Resource.Id.departureBox);
			arrival = FindViewById<TextView>(Resource.Id.arrivalBox);
			hidden = FindViewById<TextView>(Resource.Id.hiddenBox);
			icon = FindViewById<ImageView>(Resource.Id.imageView1);
			icon.Visibility = ViewStates.Invisible;
			delete = FindViewById<Button>(Resource.Id.deleteButton);
			delete.Click += Delete_Clicked;
			isRedEye = Read();
			DisplayArrivalType(isRedEye);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Delete_Clicked(object sender, EventArgs e)
		{
			File.Delete(backingFile);
			Intent returnIntent = new Intent(this, typeof(MainActivity));
			StartActivity(returnIntent);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="isRedEye"></param>
		private void DisplayArrivalType(bool isRedEye)
		{
			if (isRedEye)
			{
				hidden.Text = "This is a Red-Eye Arrival";
				icon.Visibility = ViewStates.Visible;
			}
			else
			{
				hidden.Text = "";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool Read()
		{
			//clear the box 
			arrival.Text = "";

			if (backingFile == null || !File.Exists(backingFile))
			{
				arrival.Text = "No file found";
				return false;
			}
			else
			{
				using (var reader = new StreamReader(backingFile, true))
				{
					while (reader.Peek() >= 0)
						numbers.Add(int.Parse(reader.ReadLine()));
				}

				return ConvertTime();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private bool ConvertTime()
		{
			DateTime root = DateTime.Now;
			DateTime stem = new DateTime(root.Year, root.Month, root.Day, numbers[0], numbers[1], 0);
			DateTime arrive = stem.AddMinutes(numbers[2]);
			departure.Text = String.Format("{0} at {1}", GetFormattedDate(stem.Month, stem.Day, stem.Year), GetMilitaryTime(stem.Hour, stem.Minute, 0));
			arrival.Text = String.Format("{0} at {1}", GetFormattedDate(arrive.Month, arrive.Day, arrive.Year), GetMilitaryTime(arrive.Hour, arrive.Minute, 0));
			//TimeSpan depart = new TimeSpan(root.Hour, root.Minute, 0);
			TimeSpan arrivalTime = new TimeSpan(arrive.Hour, arrive.Minute, 0);
			return DetermineRedEye(arrivalTime);
		}

		private string GetMilitaryTime(int hr, int min, int sec)
		{
			string hour = hr < 10 ? "0" + hr.ToString() : hr.ToString();
			string minute = min < 10 ? "0" + min.ToString() : min.ToString();
			string second = sec.ToString() + sec.ToString();
			string time = String.Format("{0}:{1}:{2}", hour, minute, second);
			return time;
		}

		private string GetFormattedDate(int m, int d, int y)
		{
			string year = y.ToString().Substring(2);
			string date = String.Format("{0}.{1}.{2}", m, d, year);
			return date;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="depart"></param>
		/// <param name="arrive"></param>
		/// <returns></returns>
		private bool DetermineRedEye(TimeSpan arrive)
		{
			TimeSpan min = TimeSpan.Parse("00:00");  // 12 AM
			TimeSpan max = TimeSpan.Parse("06:00");    // 6 AM

			if (arrive >= min && arrive <= max )
			{
				return true;
			}
				//if (depart < arrive && numbers[2] < 1440)
				//{
				//	return false;
				//}
				//if (depart <= arrive && numbers[2] >= 1440)
				//{
				//	return true;
				//}
				//if (depart > arrive)
				//{
				//	return true;
				//}

				return false;
		}
	}
}


//********************************************************************* SCRATCH ********************************************************************

//Enumerable.Range(0, (int) (to - from).TotalHours + 1).Select(i => from.AddHours(i)).Where(date => date.TimeOfDay >= new TimeSpan(8, 0, 0) && date.TimeOfDay <= new TimeSpan(18, 0, 0))


//string[] array = line.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
//string[] array = line.Split(',');
//foreach (string s in array)
//{
//	if (Int32.TryParse(s, out j))
//	{
//		numbers.Add(j);
//	}
//	else
//	{
//		arrival.Text = s;
//	}
//}
//DateTime root = DateTime.Now;
//DateTime stem = new DateTime(root.Year, root.Month, root.Day, numbers[0], numbers[1], 0);
//DateTime arrivalTime = stem.AddMinutes(numbers[2]);
//arrival.Text = numbers[0].ToString();
//for (int i = 0; i < numbers.Count; i++)
//{
//	arrival.Text += numbers[i] + " ";
//}
//arrival.Text = array.ToString();
//arrival.Text = line;

//using (var reader = new StreamReader(backingFile, true))
//{

//	while ((line = reader.ReadLine()) != null)
//	{
//		hidden.Text += line;
//	}
//}

//string[] array = hidden.Text.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
////string[] array = line.Split(',');
//int j;
//foreach (string s in array)
//{
//	if (Int32.TryParse(s, out j))
//	{
//		numbers.Add(j);
//	}
//	else
//	{
//		arrival.Text = s;
//	}
//}
