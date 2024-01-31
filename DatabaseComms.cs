using System;
using System.IO;
using System.Text;
using System.Net;
using MySql.Data.MySqlClient;
using FilmApp.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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

        public static string PUT(HttpListenerRequest request, string response)
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
                        response = " Table is invalid";
                        return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[HTTP.RUN.GET] Request.Body_INVALID Err: ");
                Console.WriteLine(e);
                response = "GETRequest.Body_Invalid";
                return response;
            }
            string Connection = "server=18.132.19.43;port=3306;user=film_app_user;database=film_app;password=examswillbeoversoon!";
            MyConn = new MySqlConnection(Connection);
            MyConn.Open();
            string SQLstm;
            string Table;
            switch (request.RawUrl)
            {
                case "/Movie":
                    SQLstm = "INSERT INTO Movie (MovieName, MainActor, MainDirector, ActionPref, DramaPref, RomancePref, ComedyPref) VALUES (@Name, @Actor, @Director, @Action, @Drama, @Romance, @Comedy)";
                    MyCmd = new MySqlCommand(SQLstm, MyConn);
                    MyCmd.Parameters.AddWithValue("Name", MovieName);//Variables from web request are put into the SQL statement
                    MyCmd.Parameters.AddWithValue("Actor", ActorName);
                    MyCmd.Parameters.AddWithValue("Director", DirectorName);
                    MyCmd.Parameters.AddWithValue("Action", ActionTend);
                    MyCmd.Parameters.AddWithValue("Drama", DramaTend);
                    MyCmd.Parameters.AddWithValue("Romance", RomanceTend);
                    MyCmd.Parameters.AddWithValue("Comedy", ComedyTend);
                    Table = "Movie";
                    break;
                case "/User":
                    SQLstm = "INSERT INTO User (Name, FavActor, FavDirector, Pass) VALUES (@Name, @Actor, @Director, @Pass)";
                    MyCmd = new MySqlCommand(SQLstm, MyConn);
                    MyCmd.Parameters.AddWithValue("Name", UserName);
                    MyCmd.Parameters.AddWithValue("Actor", ActorName);
                    MyCmd.Parameters.AddWithValue("Director", DirectorName);
                    MyCmd.Parameters.AddWithValue("Pass", Pass);
                    Table = "User";
                    break;
                case "/Review":
                    SQLstm = "INSERT INTO Review (UserID, MovieID, Review) VALUES (@UID, @MID, @Review)";
                    MyCmd = new MySqlCommand(SQLstm, MyConn);
                    MyCmd.Parameters.AddWithValue("UID", UserID);//Variables from web request are put into the SQL statement
                    MyCmd.Parameters.AddWithValue("MID", MovieID);
                    MyCmd.Parameters.AddWithValue("Review", ReviewScore);
                    Table = "Review";
                    break;
                default:
                    response = " Table is invalid";
                    return response;
            }
            MyCmd.Prepare();
            try
            {
                MyCmd.ExecuteNonQuery();//SQL Command is excecuted
                MyConn.Close();
                response = response + "$Added";
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
                        response = " Table is invalid";
                        return response;
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
                            if (myReader[3] == null)
                                user1.FavActor = "";
                            else
                                user1.FavActor = myReader[2].ToString();
                            if (myReader[3] == null)
                                user1.FavDirector = "";
                            else
                                user1.FavDirector = myReader[2].ToString();
                            user1.Pass = (string)myReader[4];
                            response = JObject.FromObject(user1).ToString(); ;
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
                            response = (string)JObject.FromObject(movie1);
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
            return response;
        }

        public static string GET(string RawURL, string response)
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
            MySqlCommand myCmd = new MySqlCommand();
            myCmd.Connection = MyConn;
            myCmd.CommandText = "select * from " + Table + " where " + Table + "ID = @ID";
            int IDint;
            bool Idisint = int.TryParse(ID, out IDint);
            count = 0;
            string ID1 = "";
            while (count < ID.Length)
            {
                Char = ID[count];
                if (Char == '%')
                {
                    ID1 = ID1 + " ";
                    count++;
                    count++;
                }
                else
                    ID1 = ID1 + Char.ToString();
                count++;
            }
            ID = ID1;
            if (Table == "Review")
                myCmd.CommandText = "select * from " + Table + " where UserID = @ID";
            if (Table == "Movie1")
            {
                Table = "Movie";
                myCmd.CommandText = "select * from " + Table + " where ActionPref = @Action";
                myCmd.Parameters.AddWithValue("@Action", ID);
            }
                
            if (!Idisint)
            {
                myCmd.CommandText = "select * from " + Table + " where Name = @ID";
                if (Table == "Movie")
                {
                    myCmd.CommandText = "select * from " + Table + " where MovieName = @ID";
                }
            }
            myCmd.Parameters.AddWithValue("@ID", ID);
            myCmd.Prepare();
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
                            if (myReader[3] == null)
                                user.FavActor = "";
                            else
                                user.FavActor = myReader[2].ToString();
                            if (myReader[3] == null)
                                user.FavDirector = "";
                            else
                                user.FavDirector = myReader[2].ToString();
                            user.Pass = (string)myReader[4];
                            response = JObject.FromObject(user).ToString();                         
                            if (response == null)
                            {
                                response = "No user with these credentials";
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
                            movie.ActionTend = (double)myReader[4];
                            movie.DramaTend = (double)myReader[5];
                            movie.RomanceTend = (double)myReader[6];
                            movie.ComedyTend = (double)myReader[7];
                            response = JObject.FromObject(movie).ToString();
                            if (response == "")
                            {
                                response = "No movie with that ID";
                            }
                        }
                        break;
                    case "Review":
                        myReader = myCmd.ExecuteReader();
                        List<int> MovieIDs = new List<int>();
                        while (myReader.Read())
                        {
                            MovieIDs.Add(int.Parse(myReader[2].ToString()));
                        }
                        int count1 = 0;
                        while (count1 < MovieIDs.Count)
                        {
                            response = response + MovieIDs[count1] + ", ";
                            count1++;
                        }
                        break;
                }
                MyConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                response = " Request failed";
            }
           return response;
        }

        public static string UPDATE(HttpListenerRequest request, string response)
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

        public static string DELETE(HttpListenerRequest request, string response)
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
            response = responseString;
            MyConn.Close();
            return response;
        }
    }
}
