using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs.Host;
using Azure.Data.Tables;


using Azure;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace TableStorage
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class GetTasktitle
    {
        

        [FunctionName("GetTasktitle")]
        public static async Task<IActionResult> GetTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "task")] HttpRequest req, ILogger log)
        {
            List<ClsTasks> taskList = new List<ClsTasks>();
            try
            {
                string StorageName = "tablestoragecs";
                string StorageKey = "zHcuP9GG3FeHesx7O0Lm37Otz8OE8LWNR6eCde7BS/yE86LVGm32s7jJKCVOSSts0Iz/1F6q0+qM+ASt5M8FvQ==";
                string TableName = "Todos";

                string uri = @"https://" + StorageName + ".table.core.windows.net/" + TableName;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

                int query = TableName.IndexOf("?");
                if (query > 0)
                {
                    TableName = TableName.Substring(0, query);
                }
                request = getRequestHeaders("GET", request, StorageName, StorageKey, TableName);
                string jsonData;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (System.IO.StreamReader r = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        jsonData = r.ReadToEnd();                      

                        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonData);

                        return new OkObjectResult(myDeserializedClass.value);                       
                    }
                }

            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (taskList.Count > 0)
            {
                return new OkObjectResult(taskList);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("PostTasktitleDesc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [Table("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<TodoTableEntity> todoTable, ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { PartitionKey = input.PartitionKey, RowKey = input.RowKey, sno = input.sno, title = input.title, description = input.description, imagename = input.imagename, documentname = input.documentname, Completiondate = input.Completiondate };

            await todoTable.AddAsync(todo.ToTableEntity());
            
            return new OkObjectResult(todo);
        }
        public class Todo
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public int sno { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string imagename { get; set; }
            public string documentname { get; set; }
            public DateTime Completiondate { get; set; }
            static List<Todo> items = new List<Todo>();
        }

        public class TodoCreateModel
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public int sno { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string imagename { get; set; }
            public string documentname { get; set; }
            public DateTime Completiondate { get; set; }
        }
        public static class TodoApiInMemory
        {
            static List<Todo> items = new List<Todo>();

            //...
        }

    //    [FunctionName("PostTask")]
    //    public static async Task<IActionResult> CreateTodo(
    //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posttask")] HttpRequest req, [Table("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<TodoTableEntity> todoTable, TraceWriter log)
    //    {
            
    //        log.Info("Creating a new todo list item");
    //        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    //        var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

    //        var todo = new Todo() {sno =input.sno,title=input.title,description=input.description,imagename=input.imagename,documentname=input.documentname,Completiondate=input.Completiondate};
            
    //       // await todoTable.AddAsync(todo.ToTableEntity());
    //        return new OkObjectResult(todo);
    //    }

        //[FunctionName("PostTask")]
        //public static async Task<IActionResult> CreateTasks(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posttask")] HttpRequest req,  ILogger log)
        //{

        //    string StorageName = "tablestoragecs";
        //    string StorageKey = "zHcuP9GG3FeHesx7O0Lm37Otz8OE8LWNR6eCde7BS/yE86LVGm32s7jJKCVOSSts0Iz/1F6q0+qM+ASt5M8FvQ==";
        //    string TableName = "Todos";

          

        //    var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    Root jsonData = JsonConvert.DeserializeObject<Root>(requestBody);

        //    string jsonResponse = "";
        //    string host = string.Format(@"https://{0}.table.core.windows.net/", StorageName);

        //    string resource = string.Format(@"{0}", TableName);
        //    string uri = host + resource;

        //   HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
        //    request = getRequestHeaders("POST", request, StorageName, StorageKey, resource, jsonData.Length);


        //   // var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        //    int query = TableName.IndexOf("?");
        //    if (query > 0)
        //    {
        //        TableName = TableName.Substring(0, query);
        //    }
        //    //request = getRequestHeaders("GET", request, StorageName, StorageKey, TableName);

        //    //string jsondata = JsonConvert.SerializeObject(req);
        //    request = getRequestHeaders("POST", request, StorageName, StorageKey,  TableName);

        //    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //    {
        //        streamWriter.Write(requestBody);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }
        //    //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    //{
        //    //    using (System.IO.StreamReader r = new System.IO.StreamReader(response.GetResponseStream()))
        //    //    {
        //    //        jsonResponse = r.ReadToEnd();
        //    //        // return (int)response.StatusCode;    
        //    //    }
        //    //}
        //    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(requestBody);

        //    return new OkObjectResult(myDeserializedClass.value);
           
        //}


        public static HttpWebRequest getRequestHeaders(string requestType, HttpWebRequest Newrequest, string storageAccount, string accessKey, string resource, int Length = 0)
        {
            HttpWebRequest request = Newrequest;

            switch (requestType.ToUpper())
            {
                case "GET":
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    request.ContentLength = Length;
                    request.Accept = "application/json;odata=nometadata";
                    request.Headers.Add("x-ms-date", DateTime.UtcNow.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                    request.Headers.Add("x-ms-version", "2015-04-05");
                    request.Headers.Add("Accept-Charset", "UTF-8");
                    request.Headers.Add("MaxDataServiceVersion", "3.0;NetFx");
                    request.Headers.Add("DataServiceVersion", "1.0;NetFx");
                    break;
                case "POST":
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.ContentLength = Length;
                    request.Accept = "application/json;odata=nometadata";
                    request.Headers.Add("x-ms-date", DateTime.UtcNow.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                    request.Headers.Add("x-ms-version", "2015-04-05");
                    request.Headers.Add("Accept-Charset", "UTF-8");
                    request.Headers.Add("MaxDataServiceVersion", "3.0;NetFx");
                    request.Headers.Add("DataServiceVersion", "1.0;NetFx");
                    break;
            }

            string sAuthorization = getAuthToken(request, storageAccount, accessKey, resource);
            request.Headers.Add("Authorization", sAuthorization);
            return request;
        }
        public static string getAuthToken(HttpWebRequest request, string storageAccount, string accessKey, string resource)
        {
            try
            {
                string sAuthTokn = "";

                string stringToSign = request.Headers["x-ms-date"] + "\n";

                stringToSign += "/" + storageAccount + "/" + resource;

                HMACSHA256 hasher = new HMACSHA256(Convert.FromBase64String(accessKey));

                sAuthTokn = "SharedKeyLite " + storageAccount + ":" + Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

                return sAuthTokn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
