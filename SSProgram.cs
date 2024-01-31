using System;
using System.Threading;
using MySql.Data.MySqlClient;
using FilmApp.Common;

namespace NEA_Server_App
{
    class SSProgram
    {
        static MySqlConnection MyConn;
        static MySqlCommand MyCmd;
        static void Main()
        {
            try
            {
                string Connection = "server=18.132.19.43;port=3306;user=film_app_user;database=film_app;password=examswillbeoversoon!";
                MyConn = new MySqlConnection(Connection);
                MyConn.Open();
                MyConn.Close();
                Console.WriteLine("[SQL.START] Startup_Successful");
            }
            catch (Exception e)
            {
                Console.WriteLine("[SQL.START] Startup_Failed Err: ");
                Console.WriteLine(e);
            }
            ThreadStart childref = new ThreadStart(CallToChildThread);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Console.ReadKey();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Would you like to: ");
                Console.WriteLine("A. Add a new Movie");
                Console.WriteLine("B. Find a Movie");
                Console.WriteLine("C. Delete a Movie");
                Console.WriteLine("D. Manually add a new User(Users can be added via the app)");
                Console.WriteLine("E. Find a User");
                Console.WriteLine("F. Update a User");
                Console.WriteLine("G. Delete a User");
                Console.WriteLine("H. Server Information");
                Console.WriteLine("I. Shutdown The Server");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    case "a":
                        AddMovie();
                        break;
                    case "b":
                        FindMovie();
                        break;
                    case "c":
                        DeleteMovie();
                        break;
                    case "d":
                        AddUser();
                        break;
                    case "e":
                        FindUser();
                        break;
                    case "f":
                        UpdateUser();
                        break;
                    case "g":
                        DeleteUser();
                        break;
                    case "h":
                        ServerInfo();
                        break;
                    case "i":
                        Console.WriteLine("Bye.");
                        Thread.Sleep(500);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid Option. Try Again.");
                        break;
                }
                Console.Read();
                Console.WriteLine("");
                Console.WriteLine("Press Enter to return to menu.");
                Console.Read();
            }
        }

        public static void CallToChildThread()
        {
            Console.WriteLine("[MULTI.START] Startup_Successful");
            try
            {
                Server WebServ = new Server();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("[HTTP.RUN] Uncaptured_Server_Error Err: ");
                Console.WriteLine(e);
            }
        }

        static void AddMovie()
        {
            MyConn.Open();
            Console.Clear();
            Console.Write("Enter Movie Name: ");
            string MovieName = Console.ReadLine();
            Console.Clear();
            Console.Write("Enter Main Actor Name: ");
            string ActorName = Console.ReadLine();
            Console.Clear();
            Console.Write("Enter Main Director Name: ");
            string DirectorName = Console.ReadLine();
            Console.Clear();
            Console.Write("Represent how action heavy the movie is as a number between 0 and 100: ");
            string ActionTendSTR = Console.ReadLine();
            double ActionTend = double.Parse(ActionTendSTR);
            Console.Clear();
            Console.Write("Represent how drama heavy the movie is as a number between 0 and 100: ");
            string DramaTendSTR = Console.ReadLine();
            double DramaTend = double.Parse(DramaTendSTR);
            Console.Clear();
            Console.Write("Represent how romance heavy the movie is as a number between 0 and 100: ");
            string RomanceTendSTR = Console.ReadLine();
            double RomanceTend = double.Parse(RomanceTendSTR);
            Console.Clear();
            Console.Write("Represent how action heavy the movie is as a number between 0 and 100: ");
            string ComedyTendSTR = Console.ReadLine();
            double ComedyTend = double.Parse(ComedyTendSTR);
            Console.Clear();
            string SQLstm = "INSERT INTO Movie (MovieName, MainActor, MainDirector, ActionPref, DramaPref, RomancePref, ComedyPref) VALUES(@Name, @Actor, @Director, @Action, @Drama, @Romance, @Comedy)";
            MyCmd = new MySqlCommand(SQLstm, MyConn);
            MyCmd.Parameters.AddWithValue("@Name", MovieName);
            MyCmd.Parameters.AddWithValue("@Actor", ActorName);
            MyCmd.Parameters.AddWithValue("@Director", DirectorName);
            MyCmd.Parameters.AddWithValue("@Action", ActionTend);
            MyCmd.Parameters.AddWithValue("@Drama", DramaTend);
            MyCmd.Parameters.AddWithValue("@Romance", RomanceTend);
            MyCmd.Parameters.AddWithValue("@Comedy", ComedyTend);
            MyCmd.Prepare();
            MyCmd.ExecuteNonQuery();
            MyConn.Close();
            Console.WriteLine("Movie Added");
            Console.Read();
        }

        static void FindMovie()
        {
            MyConn.Open();
            string responseString = "";
            Console.Write("Enter The ID of the Movie you want to find: ");
            string ID = Console.ReadLine();
            try
            {
                MySqlDataReader myReader = null;
                MySqlCommand myCmd = new MySqlCommand("select * from Movie where MovieID = " + ID, MyConn);
                myReader = myCmd.ExecuteReader();
                Movie movie = new Movie();
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
                    Console.WriteLine(movie.ToMovieString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseString = responseString + " Request failed";
            }
            Console.WriteLine(responseString);
            MyConn.Close();
        }


        static void DeleteMovie()
        {
            MyConn.Open();
            string responseString = "";
            Console.Write("Enter The ID of the Movie you want to delete: ");
            string ID = Console.ReadLine();
            try
            {
                MySqlCommand myCmd = new MySqlCommand("delete from Movie where MovieID = " + ID, MyConn);
                myCmd.ExecuteNonQuery();
                Console.WriteLine("Movie Deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseString = responseString + " Request failed";
            }
            Console.WriteLine(responseString);
            MyConn.Close();
        }

        static void AddUser()
        {
            bool Passcheck = false;
            string Password = "";
            string Password1 = "";
            MyConn.Open();
            Console.Clear();
            Console.Write("Enter User Name: ");
            string UserName = Console.ReadLine();
            Console.Clear();
            while (!Passcheck)
            {
                Console.Write("Enter Password: ");
                Password = Console.ReadLine();
                Console.Clear();
                Console.Write("Re-enter Password: ");
                Password1 = Console.ReadLine();
                if (Password == Password1)
                {
                    Passcheck = true;
                }
                else
                {
                    Console.WriteLine("Passwords do not match. Try again");
                }

            }
            Console.Write("Enter Favourite Actor Name: ");
            string ActorName = Console.ReadLine();
            Console.Clear();
            Console.Write("Enter Favourite Director Name: ");
            string DirectorName = Console.ReadLine();
            Console.Clear();
            string SQLstm = "INSERT INTO User (Name, FavActor, FavDirector, Pass) VALUES(@Name, @Actor, @Director, @Pass)";
            MyCmd = new MySqlCommand(SQLstm, MyConn);
            MyCmd.Parameters.AddWithValue("@Name", UserName);
            MyCmd.Parameters.AddWithValue("@Actor", ActorName);
            MyCmd.Parameters.AddWithValue("@Director", DirectorName);
            MyCmd.Parameters.AddWithValue("@Pass", Password);
            MyCmd.Prepare();
            MyCmd.ExecuteNonQuery();
            MyConn.Close();
            Console.WriteLine("User Added");
            Console.Read();
        }

        static void FindUser()
        {
            MyConn.Open();
            string responseString = "";
            Console.Write("Enter The ID of the User you want to find: ");
            string ID = Console.ReadLine();
            try
            {
                MySqlDataReader myReader = null;
                MySqlCommand myCmd = new MySqlCommand("select * from User where UserID = @ID", MyConn);
                myCmd.Parameters.AddWithValue("@ID", ID);
                myCmd.Prepare();
                myReader = myCmd.ExecuteReader();
                User user = new User();
                while (myReader.Read())
                {
                    user.ID = (int)myReader[0];
                    user.Name = (string)myReader[1];
                    user.FavActor = (string)myReader[2];
                    user.FavDirector = (string)myReader[3];
                    Console.WriteLine(user.ToUserString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseString = responseString + " Request failed";
            }
            Console.WriteLine(responseString);
            MyConn.Close();
        }

        static void UpdateUser()
        {
            MyConn.Open();
            Console.Clear();
            Console.Write("Enter ID of user you wish to change: ");
            int ID = int.Parse(Console.ReadLine());
            Console.Clear();
            MySqlDataReader myReader = null;
            MySqlCommand myCmd = new MySqlCommand("select * from User where UserID = " + ID, MyConn);
            myReader = myCmd.ExecuteReader();
            User user = new User();
            while (myReader.Read())
            {
                user.ID = (int)myReader[0];
                user.Name = (string)myReader[1];
                user.FavActor = (string)myReader[2];
                user.FavDirector = (string)myReader[3];
            }
            MyConn.Close();
            bool done = false;
            while (!done)
            {
                Console.Clear();
                Console.Write("What variable would you like to change(options: Name, FavActor, FavDirector, Done): ");
                string VarToChange = Console.ReadLine();
                switch (VarToChange.ToLower())
                {
                    case "name":
                        Console.Write("Enter new Name: ");
                        user.Name = Console.ReadLine();
                        break;
                    case "favactor":
                        Console.Write("Enter new FavActor: ");
                        user.FavActor = Console.ReadLine();
                        break;
                    case "favdirector":
                        Console.Write("Enter new FavDirector: ");
                        user.FavDirector = Console.ReadLine();
                        break;
                    case "done":
                        Console.WriteLine("Ok.");
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Option. Try Again.");
                        Thread.Sleep(500);
                        break;
                }
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
            Console.WriteLine("User Updated");
            Console.Read();
        }

        static void DeleteUser()
        {
            MyConn.Open();
            string responseString = "";
            Console.Write("Enter The ID of the User you want to delete: ");
            string ID = Console.ReadLine();
            try
            {
                MySqlCommand myCmd = new MySqlCommand("delete from User where UserID = " + ID, MyConn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                responseString = responseString + " Request failed";
            }
            Console.WriteLine(responseString);
            MyConn.Close();
        }

        public static void ServerInfo()
        {
            ThreadStart childref = new ThreadStart(ServerInfoThread);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Console.Read();
            childThread.Abort();
        }

        static void ServerInfoThread()
        {
            while (true)
            {
                Console.WriteLine("Number of Requests received and handled: " + DatabaseComms.NoOfRequests);
                Console.WriteLine("Is there a request currently being handled: " + DatabaseComms.RequestInProgress);
                Console.WriteLine("URL of server: " + DatabaseComms.ListenerURL);
                TimeSpan RunTime = DateTime.Now - DatabaseComms.StartupTime;
                Console.WriteLine("Runtime of server: " + RunTime);
                Thread.Sleep(20);
                Console.Clear();
            }
        }
    }
}