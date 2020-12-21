using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
// for the purpose of supporting lists and Intents
using System.Collections.Generic;
using Android.Content;
using PetInsights.Resources.Model;
using PetInsights.Resources.DataHelper;
using PetInsights.Resources;
using Android.Util;
using Firebase;

namespace PetInsights
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, /*Icon = "@drawable/launcher",*/
     ConfigurationChanges = Android.Content.PM.ConfigChanges.Locale /* | ConfigChanges.ScreenSize | ConfigChanges.Orientation */ )]
    public class MainActivity : AppCompatActivity
    {
          // TEMP FOR PhoneTranslator app
        static readonly List<string> phoneNumbers = new List<string>();

        // for other tutorial:
        ListView lstData;
        List<Animal> lstSource = new List<Animal>();
        DataBase db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
           
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Create DataBase (from tutorial)
            db = new DataBase();
            db.createDatabase();
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);


            // TEMP start inserting PhoneTranslator code here: // REPLACING with submitting animal information
            lstData = FindViewById<ListView>(Resource.Id.listView);        // the thing added at bottom of activity_main
            // Got the UI controls from the loaded layout
            var animalName = FindViewById<EditText>(Resource.Id.AnimalName);
            var animalAge = FindViewById<EditText>(Resource.Id.AnimalAge);
            var animalSex = FindViewById<EditText>(Resource.Id.AnimalSex);

            var submitButton = FindViewById<Button>(Resource.Id.btnAdd);
            var editButton = FindViewById<Button>(Resource.Id.btnEdit);
            var viewButton = FindViewById<Button>(Resource.Id.btnView);
            Button tutorialButton = FindViewById<Button>(Resource.Id.TutorialButton);

            // LoadData (of the tutorial)
            LoadData();




            // Event (of the tutorial)
            submitButton.Click += delegate
            {
                Animal animal = new Animal()
                {
                    Name = animalName.Text,
                    Age = int.Parse(animalAge.Text),
                    Sex = animalSex.Text
                };
                db.InsertIntoTableAnimal(animal);
                LoadData();
            };

            editButton.Click += delegate
            {
                Animal animal = new Animal()
                {
                    Id = int.Parse(animalName.Tag.ToString()),
                    Name = animalName.Text,
                    Age = int.Parse(animalAge.Text),
                    Sex = animalSex.Text
                };
                db.updateTableAnimal(animal);
                LoadData();
            };

            lstData.ItemClick += (s, e) => {
                // set background for selected item
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (e.Position == i)
                        lstData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.DarkGray);
                    else
                        lstData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                }

                // Binding Data
                var txtName = e.View.FindViewById<TextView>(Resource.Id.textView1);

                var txtAge = e.View.FindViewById<TextView>(Resource.Id.textView2);

                var txtSex = e.View.FindViewById<TextView>(Resource.Id.textView3);

                animalName.Text = txtName.Text;
                animalName.Tag = e.Id;

                animalAge.Text = txtAge.Text;
                animalSex.Text = txtSex.Text;
            };

            // Add code to translate number
            string translatedNumber = string.Empty;
           /* submitButton.Click += (sender, e) =>
            {
                // Translate user's alphanumeric phone number to numeric -- TO REPLACE
                translatedNumber = Core.PhonewordTranslator.ToNumber(animalName.Text);
                // NOTE: PhonewordTranslator comes from the new code file we created
                // that's the format of sending info to another "activity" class
                if (string.IsNullOrWhiteSpace(translatedNumber))
                {
                    animalName.Text = string.Empty;
                }
                else
                {
                    animalName.Text = translatedNumber;
                    phoneNumbers.Add(translatedNumber);
                }
            }; 
           */

            tutorialButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DogList));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };
            // TEMP end added code for phoneword testing 

        }


        // from video tutorial (need to watch beginning too)
        /*
        void InitializeFirebase()
        {
            var app = FirebaseApp init
        }*/

        private void LoadData()
        {
            lstSource = db.selectTableAnimal();
            var adapter = new ListViewAdapter(this, lstSource);
            lstData.Adapter = adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}