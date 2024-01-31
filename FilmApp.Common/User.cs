using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmApp.Common
{
    public class User
    {
       // [JsonProperty(Required = Required.Default)]
        public int? ID;
        public string Name;
        public string Pass;
        public string FavActor;
        public string FavDirector;

        public string ToUserString()
        {
            return ID + ", " + Name + ", " + FavActor + ", " + FavDirector;
        }
    }
}
