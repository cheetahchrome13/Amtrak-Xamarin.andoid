using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using static Android.Manifest;
using Android.Text;
using System;
using Android.Views;
using System.Collections.Generic;
using Android.Content;

namespace Amtrak
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
		public static string backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "contents.txt");
		List<bool> validList = new List<bool>() {false, false, false};
		LinearLayout parent;
		EditText hour;
		EditText minute;
		EditText duration;
		Button saveButton;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

			parent = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
			hour = FindViewById<EditText>(Resource.Id.hourBox);
			minute = FindViewById<EditText>(Resource.Id.minuteBox);
			duration = FindViewById<EditText>(Resource.Id.durationBox);
			hour.TextChanged += InputBox_TextChanged;
			minute.TextChanged += InputBox_TextChanged;
			duration.TextChanged += InputBox_TextChanged;

			//get save button
			saveButton = FindViewById<Button>(Resource.Id.SaveButton);
			//saveButton.Click += SaveButton_ClickAsync;
			saveButton.Click += SaveButton_Click;
			saveButton.Enabled = FormIsValid(validList);

			////get the retrieve button
			//Button getButton = FindViewById<Button>(Resource.Id.GetButton);
			//getButton.Click += GetButton_Click;

			////get delete button
			//Button delButton = FindViewById<Button>(Resource.Id.deleteButton);
			//delButton.Click += DelButton_Click;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Reanimate sender
			EditText input = sender as EditText;

			int id = input.Id;

			int lo = 00;
			int hi = 0;
			switch (id)
			{
				case Resource.Id.hourBox:
					hi = 23;
					validList[0] = InputIsValid(input, lo, hi);
					saveButton.Enabled = FormIsValid(validList);
					break;
				case Resource.Id.minuteBox:
					hi = 59;
					validList[1] = InputIsValid(input, lo, hi);
					saveButton.Enabled = FormIsValid(validList);
					break;
				case Resource.Id.durationBox:
					hi = 1500;
					validList[2] = InputIsValid(input, lo, hi);
					saveButton.Enabled = FormIsValid(validList);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="lo"></param>
		/// <param name="hi"></param>
		private bool InputIsValid(EditText input, int lo, int hi)
		{
			int time;
			
			bool empty = TextUtils.IsEmpty(input.Text);
			bool valid = Int32.TryParse(input.Text, out time);
			//bool valid = TextUtils.IsDigitsOnly(input.Text);// Useful for future projects

			if (valid)
			{
				if (time < lo || time > hi)
				{
					input.SetError($"Enter {lo} - {hi}", null);
					return false;
				}
			}

			if (empty || !valid)
			{
				input.SetError("Must enter a number", null);
				return false;
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool FormIsValid(List<bool> validList)
		{
			return validList[0] == true && validList[1] == true && validList[2] == true;
		}

		///// <summary>
		///// This is useful code for future projects
		///// </summary>
		///// <returns></returns>
		//private bool FormIsValid(LinearLayout parent)
		//{
		//	for (int i = 0; i < parent.ChildCount; i++)
		//	{
		//		View v = parent.GetChildAt(i);
		//		if (v is EditText)
		//		{
		//			//validate your EditText here
		//		}
		//	} 

		//	return true;
		//}


		//private void DelButton_Click(object sender, System.EventArgs e)
		//{
		//	File.Delete(backingFile);
		//}

		//private void GetButton_Click(object sender, System.EventArgs e)
		//{
		//	Read();
		//}

		private void SaveButton_Click(object sender, System.EventArgs e)
		{
			Write();
			Intent secondIntent = new Intent(this, typeof(SecondActivity));
			StartActivity(secondIntent);
		}

		private async System.Threading.Tasks.Task Write()
		{
			using (var writer = File.CreateText(backingFile))
			{
				await writer.WriteLineAsync(hour.Text + " ");
				await writer.WriteLineAsync(minute.Text + " ");
				await writer.WriteLineAsync(duration.Text);
			}
		}
	}
}
