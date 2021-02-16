using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Storage;
using PetInsights_all.Model;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace PetInsights_all.Services
{
    class DBFirebase
    {
        FirebaseClient client;
        FirebaseStorage firebaseStorage;
        //string curUser; // can be used for user login stuff

        public DBFirebase()
        {
            client = new FirebaseClient("https://petinsights-4dc78-default-rtdb.firebaseio.com/");
            firebaseStorage = new FirebaseStorage("petinsights-4dc78.appspot.com");
        }

        // to retrieve data from database
        public ObservableCollection<Pet> getPets()
        {
            var petData = client
                .Child("pets")
                .AsObservable<Pet>()
                .AsObservableCollection();

            // Example of querying with firebase
           /* var users =  client
              .Child("pets")
              .OrderBy("regdate")
              .StartAt("2019-01-05")
              .EndAt("2019-01-10")
              .OnceAsync<User>();
           */

            return petData;
        }

        public async Task AddPetTask(string name, int age, string imgIcon)
        {
            Pet p = new Pet() { ImgIcon = imgIcon, Name = name, Age = age };
            await client
                .Child("pets")
                .PostAsync(p);
        }


        public async Task UpdatePet(string name, int age)
        {
            var toUpdatePet = (await client
                .Child("pets")
                .OnceAsync<Pet>()).FirstOrDefault
                (a => a.Object.Name == name);

            Pet p = new Pet() { Name = name, Age = age };
            await client
                .Child("pets")
                .Child(toUpdatePet.Key)
                .PutAsync(p);
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("postImages")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;    // NOTE -- its this imageUrl that we want to save into the db
        }

        // Below code can be used for creating real user
        /*
         * public async Task AddTempUser(string location)
        {
            TempUser tu = new TempUser() { Location = location};
            var newuser = await client
                .Child("tmpusers")
                .PostAsync(tu);
            curUser = newuser.Key;
            //Console.WriteLine(:New Firebase id is " + newuser.Key);
        }
        
        public string GetDBLocation()
        {
            Console.WriteLine("In getdblocation");
            string myLoc = "";
            var petData = client
                .Child(curUser)
                .Child("Location");
            Console.WriteLine("petdata = " + petData);
            return myLoc;
        }
        */

    }
}
