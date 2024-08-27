using ABCRetail.Models;
using ABCRetail.Models.BlobStorage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace ABCRetail.Controllers
{
    using ABCRetail.Models;

    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {
            DataStorage.Initialize(configuration);
        }

        public IActionResult Index()
        {
            if (this.Request.Query["Action"] == "SignOut")
            {
                ABCRetail.UserLoggedIn = false;
                ABCRetail.UserName = string.Empty;
                ABCRetail.UserFirstName = string.Empty;
                ABCRetail.UserLastName = string.Empty;
            }

            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Contacts()
        {
            if(this.Request.Method == "POST")
            {
                string messageContent = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                DataStorage.InventoryStorage?.AddMessage(messageContent);

                // The Request succeeded.
                this.Response.StatusCode = 1;
            }

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

                StringCollection userDetails = new StringCollection();
                bool authenticated = Authenticate(userName, password, ref userDetails);

                if(accountExists && authenticated)
                {
                    ABCRetail.UserLoggedIn = true;
                    ABCRetail.UserName = userDetails[0];
                    ABCRetail.UserFirstName = userDetails[1];
                    ABCRetail.UserLastName = userDetails[2];

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

            foreach(StringCollection record in DataStorage.CustomerProfileTable)
            {
                if (record[0] == userName)
                {
                    accountExists = true;
                    break;
                }
            }

            return accountExists;
        }

        private bool Authenticate(string userName, string password, ref StringCollection userDetails)
        {
            bool result = false;

            foreach(StringCollection record in DataStorage.CustomerProfileTable)
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

        public IActionResult BuyProduct()
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
