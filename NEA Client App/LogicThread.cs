using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Common;

namespace NEA_Client_App
{
    class LogicThread
    {
        public static int UserID;
        public static int MovieID;
        public async static Task<bool> UserPassCheck(string User, string Pass)
        {
            User user = new User();
            dynamic response;
            response = await Client.GET("User/" + User);
            var responsestring = await response.Content.ReadAsStringAsync();
            user = HTTPSyntax.ReadUserJSON(responsestring);
            if (user != null && Pass == user.Pass)
            {
                UserID = (int)user.ID;
                return true;
            }
            return false;
        }

        public async static Task<bool> NewUserSetup(string User, string Pass)
        {
            User user = new User();
            user.Name = User;
            user.Pass = Pass;
            user.FavActor = null;
            user.FavDirector = null;
            dynamic response;
            response = await Client.PUT("User", user);
            response = await Client.GET("User/" + User);
            var responsestring = await response.Content.ReadAsStringAsync();
            user = HTTPSyntax.ReadUserJSON(responsestring);
            if (user.Name != null)
            {
                UserID = (int)user.ID;
                return true;
            }
            return false;
        }

        public async static Task<bool> MovieNameCheck(string moviename)
        {
            dynamic response;
            response = await Client.GET("Movie/" + moviename);
            var responsestring = await response.Content.ReadAsStringAsync();
            Movie movie = HTTPSyntax.ReadMovieJSON(responsestring);
            if (movie != null)
            {
                MovieID = movie.ID;
                return true;
            }
            return false;
        }

        public async static Task<bool> ReviewAdd(Review review)
        {
            dynamic response;
            response = await Client.PUT("Review/", review);
            var responsestring = await response.Content.ReadAsStringAsync();
            review = HTTPSyntax.ReadReviewJSON(responsestring);
            if (review != null)
                return true;
            return false;
        }
    }
}
