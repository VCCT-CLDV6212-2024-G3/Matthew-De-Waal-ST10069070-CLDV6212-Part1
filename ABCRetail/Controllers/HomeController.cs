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
    using System.Text;

    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {
            DataStorage.Initialize(configuration);
        }

        public IActionResult Index()
        {
            // Check if the request contains a 'SignOut' argument.
            if (this.Request.Query["Action"] == "SignOut")
            {
                // Sign-out the user.
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
                // Receive the message that the user wrote.
                string messageContent = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                // Add the message to ABC Retail's storage system.
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
                // Obtain the json data from the request body.
                string jsonData = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                // Deserialize the json data to an object.
                dynamic signInRequest = JsonConvert.DeserializeObject(jsonData);

                // Obtain the username and password from the object.
                string userName = Convert.ToString(signInRequest?.UserName);
                string password = Convert.ToString(signInRequest?.Password);
                bool accountExists = AccountExists(userName);

                StringCollection? userDetails = null;
                // Authenticate the user.
                bool authenticated = Authenticate(userName, password, ref userDetails);

                // Check if the authentication succeeded.
                if(accountExists && authenticated)
                {
                    // Login the user.
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
                // Obtain the jsonData from the request.
                string jsonData = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                // Create a dynamic variable from the jsonData.
                dynamic registration = JsonConvert.DeserializeObject(jsonData);

                // Obtain the data from the dynamic variable.
                string userName = Convert.ToString(registration?.UserName);
                string firstName = Convert.ToString(registration?.FirstName);
                string lastName = Convert.ToString(registration?.LastName);
                string password = Convert.ToString(registration?.Password);

                // Check if the user account exists already.
                bool accountExists = AccountExists(userName);

                if (!accountExists)
                {
                    // Continue if the user account does not exist.
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

        /// <summary>
        /// Checks if the user account exists from the given userName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool AccountExists(string userName)
        {
            bool accountExists = false;

            // Iterate through the CustomerProfileTable.
            foreach(StringCollection record in DataStorage.CustomerProfileTable)
            {
                if (record[0] == userName)
                {
                    // Assign the 'accountExists' variable to true.
                    accountExists = true;
                    break;
                }
            }

            return accountExists;
        }

        /// <summary>
        /// Authenticates the user from the provided userName and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private bool Authenticate(string userName, string password, ref StringCollection? userDetails)
        {
            bool result = false;

            // Iterate through the CustomerProfileTable.
            foreach(StringCollection record in DataStorage.CustomerProfileTable)
            {
                if (record[0] == userName)
                {
                    // Obtain the userDetails once the user account is found.
                    userDetails = record;
                    break;
                }
            }

            // Check if the user account was found and if the provided password matches
            // with the user account password.
            if (userDetails != null && userDetails[3] == password)
                result = true;

            return result;
        }
        
        public IActionResult ProductDisplay()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult BuyProduct()
        {
            if(this.Request.Method == "POST")
            {
                // Obtain the data from the request.
                string sRequestData = new StreamReader(this.Request.Body).ReadToEndAsync().Result;
                // Create a dynamic variable from sRequestData.
                dynamic requestData = JsonConvert.DeserializeObject(sRequestData);

                // Obtain the data from the dynamic variable.
                string productId = Convert.ToString(requestData?.ProductId);
                string userName = Convert.ToString(requestData?.UserName);
                string mode = Convert.ToString(requestData?.Mode);

                // Declare and instantaite a StringBuilder object.
                StringBuilder sb = new StringBuilder();

                if (mode == "OnceOff")
                {
                    // Build the report for a once-off payment.
                    sb.AppendLine("ONCE OFF PURCHASE");
                    sb.AppendLine($"Product ID: {productId}");
                    sb.AppendLine($"UserName: {userName}");
                    sb.AppendLine($"Mode: {mode}");

                    // Convert the report to a collection of bytes.
                    byte[] contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
                    // Generate a reciept filename.
                    string receiptFileName = $"receipt-{Guid.NewGuid()}.txt";

                    // Add the report to the LogStorage of ABC Retail.
                    DataStorage.LogStorage.AddItem(receiptFileName, contentBytes);
                    // Add a notification message to the TransactionStorage of ABC Retail.
                    DataStorage.TransactionStorage.AddMessage($"PRODUCT PURCHASED : {receiptFileName}");
                }

                if (mode == "Contract")
                {
                    // Build the report for a two year contract payment.
                    sb.AppendLine("TWO YEAR CONTRACT");
                    sb.AppendLine($"Product ID: {productId}");
                    sb.AppendLine($"UserName: {userName}");
                    sb.AppendLine($"Mode: {mode}");

                    // Define the duration of the contract.
                    DateTime contractBeginDate = DateTime.Now;
                    DateTime contractEndDate = DateTime.Now.AddYears(2);

                    sb.AppendLine($"Begin Date: {contractBeginDate.ToString("yyyy-MM-dd")}");
                    sb.AppendLine($"End Date: {contractEndDate.ToString("yyyy-MM-dd")}");

                    // Convert the report to a collection of bytes.
                    byte[] contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
                    // Generate a contract filename.
                    string contractFileName = $"contract-{Guid.NewGuid()}.txt";

                    // Add the report to the ContractStorage of ABC Retail.
                    DataStorage.ContractStorage.AddItem(contractFileName, contentBytes);
                    // Add a notification message to the TransactionStorage of ABC Retail.
                    DataStorage.TransactionStorage.AddMessage($"PRODUCT PURCHASED : {contractFileName}");
                }

                // The request succeeded.
                this.Response.StatusCode = 1;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
