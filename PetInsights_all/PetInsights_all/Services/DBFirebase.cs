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


        /* AddPetTask(
                petType,
                name,
                age,
                sex,
                url,
                breed.Text,
                size.Text,
                medicalCondition.Text,
                medicalConditionDetails.Text,
                personality.Text,
                pottyTrained.Text,
                apartmentFriendly.Text
        */

        // Keep in mind that some of the items (breed and onwards) may be null -- but should be initialized to Not Known (check if functioning NOTE )
        public async Task<Pet> AddPetTask(string petType, string name, int age, string sex, string imageIcon, 
                                            string breed, string size, string medicalCondition, string medicalConditionDetails, 
                                            string _personalities, string pottyTrained, string apartmentFriendly)
        {
            List<string> comments = new List<string>();
            comments.Add("");   // NOTE: temporary solution to comments section not showing up otherwise
            List<string> media = new List<string>();

            List<string> personalities = new List<string>();
            //personalities = ExtractPersonalityList(_personalities);
            personalities.Add(_personalities); 
            // NOTE - for testing purposes, not yet separating the strings

            media.Add(imageIcon);
            Pet p = new Pet();
            var newPet = await client
                .Child("pets")
                .PostAsync(p);
            string curPetKey = newPet.Key;

            p.PetType = petType;
            p.Name = name;
            p.Age = age;
            p.Sex = sex;
            p.ImgIcon = imageIcon;
            p.Breed = breed;
            p.Size = size;
            p.MedicalCondition = medicalCondition;
            p.MedicalConditionDetails = medicalConditionDetails;
            p.Personality = personalities;
            p.PottyTrained = pottyTrained;
            p.ApartmentFriendly = apartmentFriendly;
            p.PetId = curPetKey;
            p.Comments = comments;
            p.Media = media;

            await client
                 .Child("pets")
                 .Child(newPet.Key)
                 .PutAsync(p);    
            return p;

            // NOTE: this code is making two calls to the DB to insert one pet... not at all the most effective, but all I can think of so far with peptId stuff.
            // NOTE: instead of using firebase's genreated id, we could also create our own id generator and won't have to dea w this double call anymore

        }


        // NOTE: maybe move this logic into AddPet2
        public List<string> ExtractPersonalityList(string personalityList)
        {
            List<string> personalities = new List<string>();
            // add functionality to read the personalityList and separate the comments using the stated deliminator
            return personalities;
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
            try
            {
                Console.WriteLine("**filenname = " + fileName);
                var imageUrl = await firebaseStorage
                    .Child("postImages")
                    .Child(fileName)
                    .PutAsync(fileStream);
                Console.WriteLine("**upload url = " + imageUrl);
                return imageUrl;    // NOTE -- its this imageUrl that we want to save into the db
            }
            catch (Exception e)
            {
                Console.WriteLine("Upload error = " + e);
                return "";
            }
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

        public async Task AddPetMedia(Pet pet, List<string> urls)
        {
            try
            {
                Console.WriteLine("**in add pet media");
                List<string> media = pet.Media;
                Console.WriteLine("**prev media = " + pet.Media.Count);
                // NOTE url string size is 0.. why ? 
                foreach (string url in urls) {
                    Console.WriteLine("**adding a new media url");
                    media.Add(url);
                }
                Console.WriteLine("media size = " + media.Count);
                pet.Media = media;
                await client
                    .Child($"pets/{pet.PetId}")
                    .Child("Media")
                    .PutAsync(media);
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
