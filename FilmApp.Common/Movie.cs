using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmApp.Common
{
    public class Movie
    {
        public int ID;
        public string Name;
        public string Actor;
        public string Director;
        public double ActionTend;
        public double DramaTend;
        public double RomanceTend;
        public double ComedyTend;
        public string ToMovieString()
        {
            return ID + ", " + Name + ", " + Actor + ", " + Director + ", " + ActionTend + ", " + DramaTend + ", " + RomanceTend + ", " + ComedyTend;
        }
    }
}
