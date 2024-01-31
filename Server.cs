using System;
using System.Text;
using System.Net;
using FilmApp.Common;

namespace NEA_Server_App
{
    class Server
    {
        public Server()
        {
            DatabaseComms.StartupTime = DateTime.Now;
            Listener();
        }

        static void Listener()
        {
            HttpListener listener = new HttpListener();//Listener is initialized
            listener.Prefixes.Add(DatabaseComms.ListenerURL);//Address server is listening to
            listener.Start();//Server starts
            Console.WriteLine("[HTTP.START] Startup_Successful");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();//Request is received
                DatabaseComms.RequestInProgress = true;
                Response(context);//Request is Processed and response is sent
                DatabaseComms.RequestInProgress = false;
                DatabaseComms.NoOfRequests++;//Number of handled Requests
            }
        }

        static void Response(HttpListenerContext context)//Responder
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string responsestring = "";
            switch (request.RawUrl)
            {
                case "/ping":
                    responsestring = " ping!";//If a ping request is sent (represented as /ping in the URL)
                    break;
                default:
                    switch(request.HttpMethod)//divides up requests by Method
                    {
                        case "GET":
                            responsestring = DatabaseComms.GET(request.RawUrl, responsestring);
                            break;
                        case "PUT":
                            responsestring = DatabaseComms.PUT(request, responsestring);
                            break;
                        case "POST":
                            responsestring = DatabaseComms.UPDATE(request, responsestring);
                            break;
                        case "DELETE":
                            responsestring = DatabaseComms.DELETE(request, responsestring);
                            break;
                        default:
                            responsestring = "$INVALID_METHOD:" + request.HttpMethod;
                            break;
                    }
                    break;
            }
            byte[] buffer = Encoding.UTF8.GetBytes(responsestring);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);//Response is sent
            output.Close();
        }
    }
}