using System;
using System.Collections.Generic;
using System.Text;

namespace PetInsights_all.Model
{
    public class Pet
    {
        //public static string CollectionPath = "pets";

        public string ImgIcon { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string PetId { get; set; }

        public List<string> Comments { get; set; }


        // NOTE - enter other fields of the table here
    }
}
