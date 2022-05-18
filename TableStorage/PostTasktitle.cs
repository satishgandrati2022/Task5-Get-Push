

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using static TableStorage.GetTasktitle;

namespace TableStorage
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class PostTasktitle
    {
        [FunctionName("PostTasktitle")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [Table("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<TodoTableEntity> todoTable, ILogger log)
        {
          
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { PartitionKey=input.PartitionKey,RowKey=input.RowKey,sno = input.sno, title = input.title, description = input.description, imagename = input.imagename, documentname = input.documentname, Completiondate = input.Completiondate };

            await todoTable.AddAsync(todo.ToTableEntity());
            return new OkObjectResult(todo);
        }
    }

  
}
