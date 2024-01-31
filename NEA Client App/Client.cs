using System;
using System.Text;
using System.Net.Http;
using FilmApp.Common;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NEA_Client_App
{
    class Client
    {
        public static dynamic Json;

        public async static Task<HttpResponseMessage> PUT(string table, dynamic Object)
        {
            HttpContent HTTPresponse;
            string json = JObject.FromObject(Object).ToString();
            HTTPresponse = new StringContent(json);
            var client = new HttpClient();
            Json = await client.PutAsync("http://localhost:3001/" + table, HTTPresponse);
            return Json;
        }

        public async static Task<HttpResponseMessage> GET(dynamic Object)
        {
            var client = new HttpClient();
            Json = await client.GetAsync("http://localhost:3001/" + Object);

            return Json;
        }

        public async static Task<HttpResponseMessage> UPDATE(dynamic Object)
        {
            HttpContent HTTPresponse;
            HTTPresponse = JObject.FromObject(Object);
            var client = new HttpClient();
            Json = await client.PostAsync("http://localhost:3001/", HTTPresponse);
            return Json;
        }
        public async static Task<HttpResponseMessage> DELETE(dynamic Object)
        {
            var client = new HttpClient();
            Json = await client.DeleteAsync("http://localhost:3001/" + Object);
            return Json;
        }
    }
}
