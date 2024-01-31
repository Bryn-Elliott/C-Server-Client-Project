using Newtonsoft.Json;

namespace FilmApp.Common
{
    public class HTTPSyntax
    {
        public static string CreateResponseJSON(string response)
        {
            string Json = JsonConvert.SerializeObject(response);
            return Json;
        }

        public static User ReadUserJSON(string Json)
        {
            User user = JsonConvert.DeserializeObject<User>(Json);
            return user;
        }

        public static Movie ReadMovieJSON(string Json)
        {
            Movie movie = JsonConvert.DeserializeObject<Movie>(Json);
            return movie;
        }

        public static Review ReadReviewJSON(string Json)
        {
            Review review = JsonConvert.DeserializeObject<Review>(Json);
            return review;
        }

        public static string ReadResponseJSON(string Json)
        {
            string response = JsonConvert.DeserializeObject<string>(Json);
            return response;
        }
    }
}
