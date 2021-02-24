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

        public DBFirebase()
        {
            client = new FirebaseClient("https://petinsights-4dc78-default-rtdb.firebaseio.com/");
            firebaseStorage = new FirebaseStorage("petinsights-4dc78.appspot.com");
        }

        // to retrieve data from database
        public ObservableCollection<Pet> getPets()
        {
            Console.WriteLine("in get Pets");
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

        public async Task<Pet> AddPetTask(string name, int age, string imgIcon)
        {
            List<string> comments = new List<string>();
            comments.Add("");   // NOTE: temporary solution to comments section not showing up otherwise
            Pet p = new Pet();
            var newPet = await client
                .Child("pets")
                .PostAsync(p);
            string curPetKey = newPet.Key;

            p.ImgIcon = imgIcon;
            p.Name = name;
            p.Age = age;
            p.PetId = curPetKey;
            p.Comments = comments;

            Console.WriteLine("pet key = " + curPetKey);
            await client
                 .Child("pets")
                 .Child(newPet.Key)
                 .PutAsync(p);    
            return p;

            // NOTE: this code is making two calls to the DB to insert one pet... not at all the most effective, but all I can think of so far with peptId stuff.
            // NOTE: instead of using firebase's genreated id, we could also create our own id generator and won't have to dea w this double call anymore

        }


        public async Task UpdatePet(string name, int age)
        {
            var toUpdatePet = (await client
                .Child("pets")
                .OnceAsync<Pet>()).FirstOrDefault
                (a => a.Object.Name == name);

            // NOTE : NEED TO CHANGE THIS WITH ID INSTEAD

            Pet p = new Pet() { Name = name, Age = age };
            await client
                .Child("pets")
                .Child(toUpdatePet.Key)
                .PutAsync(p);
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            Console.WriteLine("in db upload file");
            var imageUrl = await firebaseStorage
                .Child("postImages")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;    // NOTE -- its this imageUrl that we want to save into the db
        }

        public async Task AddPetComment(Pet pet, string comment)
        {
            try
            {
                List<string> comments = pet.Comments;
                comments.Add(comment);

                /* await client
                     .Child($"pets/{pet.PetId}/Comments")
                     .PutAsync(comments);
                */ // this might still work

                await client
                    .Child($"pets/{pet.PetId}")
                    .Child("Comments")
                    .PutAsync(comments);
            }
            catch (Exception e)
            {
                Console.WriteLine("comment error = " + e);
            }
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
