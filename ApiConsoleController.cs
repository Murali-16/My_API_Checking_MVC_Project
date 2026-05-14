using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using PostmanReplicaMVC.Models;

namespace PostmanReplicaMVC.Controllers
{
    public class ApiConsoleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


    // ✅ GET Request
    [HttpPost]
        public async Task<string> SendGetRequest(ApiRequestModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                if (model.Headers != null)
                {
                    foreach (var header in model.Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var response = await client.GetAsync(model.Url);

                return await response.Content.ReadAsStringAsync();
            }
        }

        // POST Request
        [HttpPost]
        [ValidateInput(false)] // ✅ disable validation
        public async Task<string> SendPostRequest()
        {
            // Read RAW body (instead of model)
            string rawBody = "";
            using (var reader = new StreamReader(Request.InputStream))
            {
                rawBody = reader.ReadToEnd();
            }

            //  Get values from headers (sent from JS)
            string url = Request.Headers["Url"];
            string bodyType = Request.Headers["BodyType"];
            string headersJson = Request.Headers["CustomHeaders"];

            using (HttpClient client = new HttpClient())
            {
                //  Apply headers
                if (!string.IsNullOrEmpty(headersJson))
                {
                    var headerDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(headersJson);

                    foreach (var header in headerDict)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpContent content = null;

                if (bodyType == "json")
                {
                    content = new StringContent(rawBody, Encoding.UTF8, "application/json");
                }
                else if (bodyType == "xml")
                {
                    content = new StringContent(rawBody, Encoding.UTF8, "application/xml");
                }
                else
                {
                    content = new StringContent(rawBody, Encoding.UTF8);
                }

                var response = await client.PostAsync(url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }

}
