using PetInsights_all.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PetInsights_all
{

    public class FirebaseHelper_old
    {
        FirebaseClient firebase = new FirebaseClient("https://petinsights-4dc78.firebaseio.com");
        /*
        public async Task<List<Pet>> GetAllPets()
        {

            return (await firebase
              .Child("pets")
              .OnceAsync<Pet>()).Select(item => new Pet
              {
                  Name = item.Object.Name,
                  PetId = item.Object.PetId
              }).ToList();
        }

        public async Task AddPet(int petId, string name)
        {

            await firebase
              .Child("pets")
              .PostAsync(new Pet() { PetId = petId, Name = name });
        }

        public async Task<Pet> GetPets(int petId)
        {
            var allPets = await GetAllPets();
            await firebase
              .Child("pets")
              .OnceAsync<Pet>();
            return allPets.Where(a => a.PetId == petId).FirstOrDefault();
        }

        public async Task UpdatePet(int petId, string name)
        {
            var toUpdatePet = (await firebase
              .Child("pets")
              .OnceAsync<Pet>()).Where(a => a.Object.PetId == petId).FirstOrDefault();

            await firebase
              .Child("pets")
              .Child(toUpdatePet.Key)
              .PutAsync(new Pet() { PetId = petId, Name = name });
        }

        public async Task DeletePet(int petId)
        {
            var toDeletePet = (await firebase
              .Child("pets")
              .OnceAsync<Pet>()).Where(a => a.Object.PetId == petId).FirstOrDefault();
            await firebase.Child("pets").Child(toDeletePet.Key).DeleteAsync();

        }
        */
    }

}