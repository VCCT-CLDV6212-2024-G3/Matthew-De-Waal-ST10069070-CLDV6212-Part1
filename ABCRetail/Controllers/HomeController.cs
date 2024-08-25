using ABCRetail.Models;
using ABCRetail.Models.BlobStorage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace ABCRetail.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {
            DataStorage.Initialize(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult SignIn()
        {
            if (this.Request.Method == "POST")
            {
                string jsonData = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                dynamic signInRequest = JsonConvert.DeserializeObject(jsonData);

                string userName = Convert.ToString(signInRequest?.UserName);
                string password = Convert.ToString(signInRequest?.Password);

                bool accountExists = AccountExists(userName);
                bool authenticated = Authenticate(userName, password);

                if(accountExists && authenticated)
                {
                    // The request succeeded
                    this.Response.StatusCode = 1;
                }
                else
                {
                    // The request failed
                    this.Response.StatusCode = 2;
                }

            }

            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult SignUp()
        {
            if(this.Request.Method == "POST")
            {
                string jsonData = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                dynamic registration = JsonConvert.DeserializeObject(jsonData);

                string userName = Convert.ToString(registration?.UserName);
                string firstName = Convert.ToString(registration?.FirstName);
                string lastName = Convert.ToString(registration?.LastName);
                string password = Convert.ToString(registration?.Password);

                bool accountExists = AccountExists(userName);

                if (!accountExists)
                {
                    string[] userAccount = { userName, firstName, lastName, password };
                    DataStorage.CustomerProfileTable?.AddRecord(userAccount);

                    // The request succeeded.
                    this.Response.StatusCode = 1;
                }
                else
                {
                    // The reuqest failed.
                    this.Response.StatusCode = 2;
                }
            }

            return View();
        }

        private bool AccountExists(string userName)
        {
            bool accountExists = false;

            foreach(string[] record in DataStorage.CustomerProfileTable)
            {
                if (record[0] == userName)
                {
                    accountExists = true;
                    break;
                }
            }

            return accountExists;
        }

        private bool Authenticate(string userName, string password)
        {
            bool result = false;
            string[]? userDetails = null;

            foreach(string[] record in DataStorage.CustomerProfileTable)
            {
                if (record[0] == userName)
                {
                    userDetails = record;
                    break;
                }
            }

            if (userDetails != null && userDetails[3] == password)
                result = true;

            return result;
        }
        
        public IActionResult ProductDisplay()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
