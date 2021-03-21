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
        ObservableCollection<Pet> _pets;

        // NOTE - temporary values for testing
        //string _ageRange = "0 - 6 months";
        string _address = "Cincinnati, Ohio";
        string _hypoallergenic = "No";
        string _sheds = "Yes";
        string _maintenance = "Low";
        string _affiliation = "SAAP";

        public DBFirebase()
        {
            client = new FirebaseClient("https://petinsights-4dc78-default-rtdb.firebaseio.com/");
            firebaseStorage = new FirebaseStorage("petinsights-4dc78.appspot.com");
            _pets = new ObservableCollection<Pet>();
        }

        // to retrieve data from database
        public ObservableCollection<Pet> getPets()
        {
            var petData = client
                .Child("pets")
                .AsObservable<Pet>()
                .AsObservableCollection();

            //_pets = petData;
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

        public ObservableCollection<Pet> GetAffiliatedPets(ObservableCollection<Pet> petList, Pet _pet)
        {
            ObservableCollection<Pet> _sharedpets = new ObservableCollection<Pet>();
            foreach (Pet pet in petList)
            {
                if (pet.Affiliation == _pet.Affiliation && pet.Name != _pet.Name)
                {
                    _sharedpets.Add(pet);
                }
            }
            return _sharedpets;
        }


  
        // Keep in mind that some of the items (breed and onwards) may be null -- but should be initialized to Not Known (check if functioning NOTE )
        public async Task<Pet> AddPetTask(string petType, string name, int age, string sex, string imageIcon, 
                                            string breed, string size, string medicalCondition, string medicalConditionDetails, 
                                            string _personalities, string pottyTrained, string apartmentFriendly)
        {
            ObservableCollection<string> comments = new ObservableCollection<string>();
            //List<string> comments = new List<string>();
            comments.Add("");   // NOTE: temporary solution to comments section not showing up otherwise - need to display from count 1+

            ObservableCollection<string> media = new ObservableCollection<string>();
            //List<string> media = new List<string>();

            List<string> personalities = new List<string>();
            //personalities = ExtractPersonalityList(_personalities);       // TO IMPLEMENT
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

            // autopopulated fields:
            p.AgeRange = getAgeRange(age);
            p.Address = _address;
            p.Hypoallergenic = _hypoallergenic;
            p.Sheds = _sheds;
            p.Maintenance = _maintenance;
            p.Affiliation = _affiliation;



            await client
                 .Child("pets")
                 .Child(newPet.Key)
                 .PutAsync(p);    
            return p;

            // NOTE: this code is making two calls to the DB to insert one pet... not at all the most effective, but all I can think of so far with peptId stuff.
            // NOTE: instead of using firebase's genreated id, we could also create our own id generator and won't have to dea w this double call anymore

        }

        private string getAgeRange(int age)
        {
            string rng = "";
            if (age >= 0 && age <= .5)
            {
                rng = "newborn"; // puppy/kitten
            }
            else if (age > .5 && age <= 2)
            {
                rng = "young"; // young
            }
            else if (age > 2 && age <= 8)
            {
                rng = "adult"; // middle age
            }
            else if (age > 8)
            {
                rng = "senior"; // senior
            }
            return rng;
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
                var imageUrl = await firebaseStorage
                    .Child("postImages")
                    .Child(fileName)
                    .PutAsync(fileStream);
                return imageUrl;    // the imageUrl we want to save into the db
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
                ObservableCollection<string> comments = pet.Comments;
                if(comments.Count == 1 && comments[0].Equals(""))
                {
                    Console.WriteLine("in db removing blank comment string ");
                    comments.Remove("");
                }
                //List<string> comments = pet.Comments;
                comments.Add(comment);

                pet.Comments = comments;

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
                ObservableCollection<string> media = pet.Media;
                //List<string> media = pet.Media;
                foreach (string url in urls) {
                    media.Add(url);
                }
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
