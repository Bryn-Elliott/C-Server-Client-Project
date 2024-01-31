using System;
using System.IO;
using System.Text;
using System.Net;
using MySql.Data.MySqlClient;
using FilmApp.Common;
using Newtonsoft.Json.Linq;

namespace NEA_Server_App
{
    class DatabaseComms
    {
        static MySqlConnection MyConn;
        static MySqlCommand MyCmd;
        public static int NoOfRequests = 0;
        public static bool RequestInProgress = false;
        public static string ListenerURL = "http://localhost:3001/";
        public static string Connection = "server=18.132.19.43;port=3306;user=film_app_user;database=film_app;password=examswillbeoversoon!";
        public static DateTime StartupTime;

        public static Response PUT(HttpListenerRequest request, Response HTTPresponse)
        {
            string documentContents = null;
            string MovieName = null;
            string ActorName = null;
            string DirectorName = null;
            string Pass = null;
            double ActionTend = 0;
            double DramaTend = 0;
            double RomanceTend = 0;
            double ComedyTend = 0;
            string UserName = null;
            int UserID = 0;
            int MovieID = 0;
            int ReviewScore = 0;
            Movie movie;
            User user;
            Review review;
            try
            {
                using (Stream receiveStream = request.InputStream)//Format of Movie Body = {MovieName,ActorName,DirectorName,ActionTend,DramaTend,RomanceTend,ComedyTend}
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                switch (request.RawUrl)
                {
                    case "/Movie":
                        movie = HTTPSyntax.ReadMovieJSON(documentContents);
                        MovieName = movie.Name;
                        ActorName = movie.Actor;
                        DirectorName = movie.Director;
                        ActionTend = movie.ActionTend;
                        DramaTend = movie.DramaTend;
                        RomanceTend = movie.RomanceTend;
                        ComedyTend = movie.ComedyTend;
                        break;
                    case "/User":
                        user = HTTPSyntax.ReadUserJSON(documentContents);
                        UserName = user.Name;
                        ActorName = user.FavActor;
                        DirectorName = user.FavDirector;
                        Pass = user.Pass;
                        break;
                    case "/Review":
                        review = HTTPSyntax.ReadReviewJSON(documentContents);
                        UserID = review.UserID;
                        MovieID = review.MovieID;
                        ReviewScore = review.Score;
                        break;
                    default:
                        HTTPresponse.Object = " Table is invalid";
                        return HTTPresponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[HTTP.RUN.GET] Request.Body_INVALID Err: ");
                Console.WriteLine(e);
                HTTPresponse.Object = "GETRequest.Body_Invalid";
                return HTTPresponse;
            }
            string Connection = "server=18.132.19.43;port=3306;user=film_app_user;database=film_app;password=examswillbeoversoon!";
            MyConn = new MySqlConnection(Connection);
            MyConn.Open();
            string SQLstm;
            string Table;
            switch (request.RawUrl)
            {
                case "/Movie":
                    SQLstm = "INSERT INTO Movie (MovieName, MainActor, MainDirector, ActionPref, DramaPref, RomancePref, ComedyPref) VALUES(@Name, @Actor, @Director, @Action, @Drama, @Romance, @Comedy)";
                    MyCmd = new MySqlCommand(SQLstm, MyConn);
                    MyCmd.Parameters.AddWithValue("@Name", MovieName);//Variables from web request are put into the SQL statement
                    MyCmd.Parameters.AddWithValue("@Actor", ActorName);
                    MyCmd.Parameters.AddWithValue("@Director", DirectorName);
                    MyCmd.Parameters.AddWithValue("@Action", ActionTend);
                    MyCmd.Parameters.AddWithValue("@Drama", DramaTend);
                    MyCmd.Parameters.AddWithValue("@Romance", RomanceTend);
                    MyCmd.Parameters.AddWithValue("@Comedy", ComedyTend);
                    Table = "Movie";
                    break;
                case "/User":
                    SQLstm = "INSERT INTO User (Name, FavActor, FavDirector, Pass) VALUES(@Name, @Actor, @Director, @Pass)";
                    MyCmd = new MySqlCommand(SQLstm, MyConn);
                    MyCmd.Parameters.AddWithValue("@Name", UserName);
                    MyCmd.Parameters.AddWithValue("@Actor", ActorName);
                    MyCmd.Parameters.AddWithValue("@Director", DirectorName);
                    MyCmd.Parameters.AddWithValue("@Pass", Pass);
                    Table = "User";
                    break;
                case "/Review":
                    SQLstm = "INSERT INTO Review (UserID, MovieID, Review) VALUES(@UID, @MID, @Review)";
                    MyCmd = new MySqlCommand(SQLstm, MyConn);
                    MyCmd.Parameters.AddWithValue("@UID", UserID);//Variables from web request are put into the SQL statement
                    MyCmd.Parameters.AddWithValue("@MID", MovieID);
                    MyCmd.Parameters.AddWithValue("@Review", ReviewScore);
                    Table = "Review";
                    break;
                default:
                    HTTPresponse.Object = " Table is invalid";
                    return HTTPresponse;
            }
            MyCmd.Prepare();
            try
            {
                MyCmd.ExecuteNonQuery();//SQL Command is excecuted
                MyConn.Close();
                HTTPresponse.Object = HTTPresponse.Object + "$Added";
                MyConn.Open();
                switch (Table)
                {
                    case "Movie":
                        SQLstm = ("select * from " + Table + " where MovieID = LAST_INSERT_ID()");
                        break;
                    case "User":
                        SQLstm = ("select * from " + Table + " where UserID = LAST_INSERT_ID()");
                        break;
                    case "Review":
                        SQLstm = ("select * from " + Table + " where ReviewID = LAST_INSERT_ID()");
                        break;
                    default:
                        HTTPresponse.Object = " Table is invalid";
                        return HTTPresponse;
                }
                MyCmd = new MySqlCommand(SQLstm, MyConn);
                MyCmd.Parameters.AddWithValue("@table", Table);
                MySqlDataReader myReader = null;
                switch (Table)
                {
                    case "User":
                        User user1 = new User();
                        myReader = MyCmd.ExecuteReader();
                        while (myReader.Read())
                        {
                            user1.ID = (int)myReader[0];
                            user1.Name = (string)myReader[1];
                            user1.FavActor = (string)myReader[2];
                            user1.FavDirector = (string)myReader[3];
                            HTTPresponse.Object = JObject.FromObject(user1);
                        }
                        break;
                    case "Movie":
                        Movie movie1 = new Movie();
                        myReader = MyCmd.ExecuteReader();
                        while (myReader.Read())
                        {
                            movie1.ID = (int)myReader[0];
                            movie1.Name = (string)myReader[1];
                            movie1.Actor = (string)myReader[2];
                            movie1.Director = (string)myReader[3];
                            movie1.ActionTend = (double)myReader[4];
                            movie1.DramaTend = (double)myReader[5];
                            movie1.RomanceTend = (double)myReader[6];
                            movie1.ComedyTend = (double)myReader[7];
                            HTTPresponse.Object = JObject.FromObject(movie1);
                        }
                        break;
                }
                MyConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[SQL.RUN.PUT] INSERT_Request_Failed Err:");
                Console.WriteLine(e);
            }
            return HTTPresponse;
        }

        public static Response GET(string RawURL, Response HTTPresponse)
        {
            char Char = RawURL[1];
            int count = 1;
            string Table = "";
            while (Char != '/')
            {
                Table = Table + Char.ToString();
                count++;
                Char = RawURL[count];
            }
            string ID = "";
            count++;
            Char = RawURL[count];
            while (Char != '/' && count < RawURL.Length)
            {
                Char = RawURL[count];
                ID = ID + Char.ToString();
                count++;
            }
            MyConn = new MySqlConnection(Connection);
            MyConn.Open();
            MySqlCommand myCmd = null;
            switch (Table)
            {
                case "Movie":
                    myCmd = new MySqlCommand("select * from " + Table + " where MovieID = " + ID, MyConn);
                    break;
                case "User":
                    int IDint;
                    bool IDIsInt = int.TryParse(ID, out IDint);
                    if (IDIsInt)
                    {
                        myCmd = new MySqlCommand("select * from " + Table + " where UserID = " + ID, MyConn);
                    }
                    else
                    {
                        myCmd = new MySqlCommand("select * from " + Table + " where Name = " + ID, MyConn);
                    }
                    break;
                case "Review":
                    myCmd = new MySqlCommand("select * from " + Table + " where ReviewID = " + ID, MyConn);
                    break;
                default:
                    HTTPresponse.Object = " Table is invalid";
                    return HTTPresponse;
            }
            try
            {
                MySqlDataReader myReader = null;
                switch (Table)
                {
                    case "User":
                        User user = new User();
                        myReader = myCmd.ExecuteReader();
                        while (myReader.Read())
                        {
                            user.ID = (int)myReader[0];
                            user.Name = (string)myReader[1];
                            user.FavActor = (string)myReader[2];
                            user.FavDirector = (string)myReader[3];
                            HTTPresponse.Object = JObject.FromObject(user);
                            if (HTTPresponse.Object = "")
                            {
                                HTTPresponse.Object = "No user with these credentials";
                            }
                        }
                        break;
                    case "Movie":
                        Movie movie = new Movie();
                        myReader = myCmd.ExecuteReader();
                        while (myReader.Read())
                        {
                            movie.ID = (int)myReader[0];
                            movie.Name = (string)myReader[1];
                            movie.Actor = (string)myReader[2];
                            movie.Director = (string)myReader[3];
                            movie.ActionTend = (double)myReader[4];
                            movie.DramaTend = (double)myReader[5];
                            movie.RomanceTend = (double)myReader[6];
                            movie.ComedyTend = (double)myReader[7];
                            HTTPresponse.Object = JObject.FromObject(movie);
                            if (HTTPresponse.Object = "")
                            {
                                HTTPresponse.Object = "No movie with that ID";
                            }
                        }
                        break;
                }
                MyConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                HTTPresponse.Object = " Request failed";
            }
            return HTTPresponse;
        }

        public static Response UPDATE(HttpListenerRequest request, Response HTTPresponse)
        {
            string Connection = "server=18.132.19.43;port=3306;user=film_app_user;database=film_app;password=examswillbeoversoon!";
            MyConn = new MySqlConnection(Connection);
            MyConn.Open();
            char Char;
            int count = 1;
            string ID = "";
            string Table = "";
            string RawURL = request.RawUrl;
            Char = RawURL[count];
            while (Char != '/' && count < RawURL.Length)
            {
                Table = Table + Char.ToString();
                count++;
                Char = RawURL[count];
            }
            count++;
            while (count < RawURL.Length)
            {
                Char = RawURL[count];
                ID = ID + Char.ToString();
                count++;
            }
            MySqlDataReader myReader = null;
            MySqlCommand myCmd = new MySqlCommand("select * from User where UserID = " + ID, MyConn);
            myReader = myCmd.ExecuteReader();
            User user = new User();
            while (myReader.Read())
            {
                user.Name = (string)myReader[1];
                user.FavActor = (string)myReader[2];
                user.FavDirector = (string)myReader[3];
            }
            MyConn.Close();
            string documentContents;
            using (Stream receiveStream = request.InputStream)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    documentContents = readStream.ReadToEnd();
                }
            }
            User user1 = HTTPSyntax.ReadUserJSON(documentContents);
            if (user1.Name != null)
            {
                user.Name = user1.Name;
            }
            if (user1.FavActor != null)
            {
                user.FavActor = user1.FavActor;
            }
            if (user1.FavDirector != null)
            {
                user.FavDirector = user1.FavDirector;
            }
            MyConn.Open();
            string SQLstm = "UPDATE User SET Name = @Name, FavActor = @Actor, FavDirector = @Director WHERE UserID = @ID";
            MyCmd = new MySqlCommand(SQLstm, MyConn);
            MyCmd.Parameters.AddWithValue("@ID", ID);
            MyCmd.Parameters.AddWithValue("@Name", user.Name);
            MyCmd.Parameters.AddWithValue("@Actor", user.FavActor);
            MyCmd.Parameters.AddWithValue("@Director", user.FavDirector);
            MyCmd.Prepare();
            MyCmd.ExecuteNonQuery();
            MyConn.Close();
            Console.Read();
            return null;
        }

        public static Response DELETE(HttpListenerRequest request, Response HTTPresponse)
        {
            MyConn = new MySqlConnection(Connection);
            string RawURL = request.RawUrl;
            char Char;
            int count = 1;
            string ID = "";
            string Table = "";
            Char = RawURL[count];
            while (Char != '/' && count < RawURL.Length)
            {
                Table = Table + Char.ToString();
                count++;
                Char = RawURL[count];
            }
            count++;
            while (count < RawURL.Length)
            {
                Char = RawURL[count];
                ID = ID + Char.ToString();
                count++;
            }
            MyConn.Open();
            string responseString = "";
            try
            {
                switch (Table)
                {
                    case "User":
                        MySqlCommand myCmd = new MySqlCommand("DELETE FROM User WHERE UserID = " + ID, MyConn);
                        myCmd.ExecuteNonQuery();
                        responseString = "Movie Deleted";
                        break;
                    case "Movie":
                        MySqlCommand MyCmd = new MySqlCommand("DELETE FROM Movie WHERE MovieID = " + ID, MyConn);
                        MyCmd.ExecuteNonQuery();
                        responseString = "Movie Deleted";
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseString = "DELETE Request failed";
            }
            HTTPresponse.Object = responseString;
            MyConn.Close();
            return HTTPresponse;
        }
    }
}
